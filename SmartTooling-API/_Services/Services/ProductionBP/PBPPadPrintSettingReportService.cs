using AutoMapper;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using SmartTooling_API.Models.ProductionBP;
using SmartTooling_API.Models.SmartTool;
using SmartTooling_API.Helpers.Utilities;

namespace SmartTooling_API._Services.Services.ProductionBP
{
    public class PBPPadPrintSettingReportService : IPBPPadPrintSettingReportService
    {
        private readonly IModelRepository _modelRepo;
        private readonly IPBPPadPrintSettingRepository _iPBPPadPrintSettingRepo;
        private readonly IPBPMachineVendorTypeRepository _pBPMachineVendorTypeRepo;
        private readonly IImageUrlUtility _imageUrlUtility;
        private readonly IPBPMaterialTypeRepository _pBPMaterialTypeRepo;
        private readonly IPBPPadShapeTypeRepository _pBPPadShapeTypeRepo;
        private readonly IFactoryRepository _factoryRepo;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public PBPPadPrintSettingReportService(IConfiguration configuration,
        IWebHostEnvironment webHostEnvironment,
        IMapper mapper, IModelRepository modelRepo,
        IFactoryRepository factoryRepo,
        IPBPPadPrintSettingRepository iPBPPadPrintSettingRepo,
         IPBPPadShapeTypeRepository pBPPadShapeTypeRepo,
         IPBPMaterialTypeRepository pBPMaterialTypeRepo,
         IPBPMachineVendorTypeRepository pBPMachineVendorTypeRepo,
         IImageUrlUtility imageUrlUtility
        )
        {
            _factoryRepo = factoryRepo;
            _modelRepo = modelRepo;
            _iPBPPadPrintSettingRepo = iPBPPadPrintSettingRepo;
            _pBPPadShapeTypeRepo = pBPPadShapeTypeRepo;
            _pBPMachineVendorTypeRepo = pBPMachineVendorTypeRepo;
            _imageUrlUtility = imageUrlUtility;
            _pBPMaterialTypeRepo = pBPMaterialTypeRepo;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllFactory()
        {
            var data = await _factoryRepo.FindAll()
                .Select(x => new KeyValuePair<string, string>(x.factory_id.Trim(), x.factory_name.Trim()))
                .Distinct()
                .ToListAsync();
            return data;
        }

        public async Task<PageListUtility<PBP_Pad_Print_SettingDTO>> Search(PaginationParams pagination, PBPPadPrintSettingParams searchParam, bool isPaging = true)
        {
            List<PBP_Pad_Print_SettingDTO> result = new List<PBP_Pad_Print_SettingDTO>();
            if (!string.IsNullOrEmpty(searchParam.factory_id?.Trim()))
            {
                var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(searchParam.factory_id);
                result = await SearchDataEachFactory(searchParam, searchParam.factory_id, imageUrl);
            }
            else
            {
                List<string> FactoryList = new List<string> { "SHC", "CB", "TSH", "SPC" };
                foreach (var item in FactoryList)
                {
                    var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(item);
                    var data = await SearchDataEachFactory(searchParam, item, imageUrl);
                    result.AddRange(data);
                }

            }
            return PageListUtility<PBP_Pad_Print_SettingDTO>.PageList(result, pagination.PageNumber, pagination.PageSize, isPaging);
        }

        private async Task<List<PBP_Pad_Print_SettingDTO>> SearchDataEachFactory(PBPPadPrintSettingParams searchParam, string factory, string imageUrl)
        {
            var predPadPrint = PredicateBuilder.New<PBP_Pad_Print_Setting>(true);
            var predModel = PredicateBuilder.New<Model>(true);
            _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
            predPadPrint = predPadPrint.And(x => x.factory_id.Trim() == factory.Trim());
            if (!string.IsNullOrEmpty(searchParam.dev_season?.Trim()))
            {
                predModel = predModel.And(x => x.dev_season.ToUpper().Trim() == searchParam.dev_season.ToUpper().Trim());
            }
            if (!string.IsNullOrEmpty(searchParam.production_season?.Trim()))
            {
                predModel = predModel.And(x => x.prod_season.ToUpper().Trim() == searchParam.production_season.ToUpper().Trim());
            }

            var pBPPadPrintQuery = _iPBPPadPrintSettingRepo.FindAll(predPadPrint);
            var modelQuery = _modelRepo.FindAll(predModel);
            var pBPPadShapeQuery = _pBPPadShapeTypeRepo.FindAll();
            var pBPMachineVendorQuery = _pBPMachineVendorTypeRepo.FindAll();
            var pBPMaterialTypeQuery = _pBPMaterialTypeRepo.FindAll();

            var data = pBPPadPrintQuery
                .Join(modelQuery,
                  x => new { x.factory_id, x.model_no },
                  y => new { y.factory_id, y.model_no },
                  (x, y) => new { a = x, b = y })
                .GroupJoin(pBPPadShapeQuery,
                  x => new { x.a.factory_id, x.a.pad_shape_id },
                  y => new { y.factory_id, y.pad_shape_id },
                  (x, y) => new { x.a, x.b, d = y }
                ).SelectMany(x => x.d.DefaultIfEmpty(), (x, y) => new PBP_Pad_Print_SettingDTO
                {
                    dev_season = x.b.dev_season,
                    production_season = x.b.prod_season,
                    model_no = x.a.model_no,
                    model_name = x.b.model_name,
                    article_no_remarks = x.a.article_no_remarks,
                    component_name = x.a.component_name,
                    material_description = x.a.material_description,
                    pad_shape_name = y.pad_shape_name,
                    number_of_pad_hits = x.a.number_of_pad_hits,
                    article_no_is_general = x.a.article_no_is_general,
                    chemical_additive = x.a.chemical_additive,
                    chemical_hardener = x.a.chemical_hardener,
                    chemical_ink = x.a.chemical_ink,
                    chemical_others = x.a.chemical_others,
                    chemical_primer = x.a.chemical_primer,
                    component_photo_url = x.a.component_photo_url,
                    create_by = x.a.create_by,
                    create_time = x.a.create_time,
                    factory_id = x.a.factory_id,
                    is_rotary_table_used = x.a.is_rotary_table_used,
                    machine_model = x.a.machine_model,
                    machine_vendor_id = x.a.machine_vendor_id,
                    material_type_id = x.a.material_type_id,
                    operation_video_url = x.a.operation_video_url,
                    pad_shape_id = x.a.pad_shape_id,
                    update_by = x.a.update_by,
                    update_time = x.a.update_time,
                    machine_vendor_name = pBPMachineVendorQuery.Where(y => y.machine_vendor_id == x.a.machine_vendor_id)
                                         .Select(y => y.machine_vendor_name)
                                         .FirstOrDefault(),
                    material_type_name = pBPMaterialTypeQuery.Where(y => y.material_type_id == x.a.material_type_id)
                                         .Select(y => y.material_type_name)
                                         .FirstOrDefault(),
                    article_no_is_general_result = x.a.article_no_is_general ? "YES" : "NO",
                    is_rotary_table_used_result = x.a.is_rotary_table_used ? "YES" : "NO",
                    component_photo_url_result = imageUrl + x.a.component_photo_url,
                    operation_video_url_result = imageUrl + x.a.operation_video_url

                });
            if (!string.IsNullOrEmpty(searchParam.model))
            {
                data = data.Where(x => x.model_no.Contains(searchParam.model) || x.model_name.Contains(searchParam.model));
            }
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return await data.ToListAsync();
        }
    }
}