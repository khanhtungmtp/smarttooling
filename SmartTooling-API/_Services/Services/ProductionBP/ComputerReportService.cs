using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;
using SmartTooling_API.Helpers.Utilities;
using SmartTooling_API.Models.ProductionBP;
using SmartTooling_API.Models.SmartTool;
namespace SmartTooling_API._Services.Services.ProductionBP
{
    public class ComputerReportService : IComputerReportService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private readonly IFactoryRepository _factoryRepository;
        private readonly IPBPComputerStitchingSettingRepository _pBPComputerStitchingSettingRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IModelOperationRepository _modelOperationRepository;
        private readonly IPBPCSTypeRepository _pBPCSTypeRepository;
        private readonly IPBPCSMachineTypeRepository _pBPCSMachineTypeRepository;
        private readonly IPBPMainUpperMaterialTypeRepository _pBPMainUpperMaterialTypeRepository;
        private readonly IPBPJigDesignTypeRepository _pBPJigDesignTypeRepository;
        private readonly IImageUrlUtility _imageUrlUtility;

        public ComputerReportService(IMapper mapper, MapperConfiguration configMapper, IConfiguration configuration,
            IFactoryRepository factoryRepository, IPBPComputerStitchingSettingRepository pBPComputerStitchingSettingRepository,
            IModelRepository modelRepository, IStageRepository stageRepository,
            IModelOperationRepository modelOperationRepository, IPBPCSTypeRepository pBPCSTypeRepository,
            IPBPCSMachineTypeRepository pBPCSMachineTypeRepository,
            IPBPMainUpperMaterialTypeRepository pBPMainUpperMaterialTypeRepository,
            IPBPJigDesignTypeRepository pBPJigDesignTypeRepository, IImageUrlUtility imageUrlUtility)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _configuration = configuration;
            _factoryRepository = factoryRepository;
            _pBPComputerStitchingSettingRepository = pBPComputerStitchingSettingRepository;
            _modelRepository = modelRepository;
            _stageRepository = stageRepository;
            _modelOperationRepository = modelOperationRepository;
            _pBPCSTypeRepository = pBPCSTypeRepository;
            _pBPCSMachineTypeRepository = pBPCSMachineTypeRepository;
            _pBPMainUpperMaterialTypeRepository = pBPMainUpperMaterialTypeRepository;
            _pBPJigDesignTypeRepository = pBPJigDesignTypeRepository;
            _imageUrlUtility = imageUrlUtility;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllFactory()
        {
            var data = await _factoryRepository.FindAll().Select(x => new KeyValuePair<string, string>(x.factory_id.Trim(), x.factory_name.Trim())).Distinct().ToListAsync();
            return data;
        }

        public async Task<PageListUtility<PBP_ComputerStitchingSettingDTO>> Search(PaginationParams pagination, ComputerReportParam searchParam, bool isPagging)
        {
            List<string> listFactories = new List<string>() { "SHC", "CB", "SPC", "TSH" };
            var newLists = new List<PBP_ComputerStitchingSettingDTO>();

            if (searchParam.factory == "All")
            {
                for (int i = 0; i < listFactories.Count; i++)
                {
                    var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(listFactories[i]);
                    var data = await GetDataSearch(pagination, searchParam, listFactories[i], imageUrl, false);
                    newLists.AddRange(data.Result);
                }
                return PageListUtility<PBP_ComputerStitchingSettingDTO>.PageList(newLists, pagination.PageNumber, pagination.PageSize, isPagging);
            }
            else
            {
                var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(searchParam.factory);
                return await GetDataSearch(pagination, searchParam, searchParam.factory, imageUrl, isPagging);
            }
        }

        public async Task<PageListUtility<PBP_ComputerStitchingSettingDTO>> GetDataSearch(PaginationParams pagination, ComputerReportParam searchParam, string factory,string imageUrl, bool isPagging = true)
        {
            var pred = PredicateBuilder.New<PBP_ComputerStitchingSetting>(true);
            var predModel = PredicateBuilder.New<Model>(true);
            _configuration.GetSection("AppSettings:DataSeach").Value = factory;
            
            
            var dataStage = _stageRepository.FindAll();
            var dataModelOperation = _modelOperationRepository.FindAll();
            var dataCSType = _pBPCSTypeRepository.FindAll();
            var dataCSMachineType = _pBPCSMachineTypeRepository.FindAll();
            //detail
            var dataMainUpper = _pBPMainUpperMaterialTypeRepository.FindAll();
            var dataJigDesign = _pBPJigDesignTypeRepository.FindAll();

            if (!string.IsNullOrEmpty(searchParam.factory) && searchParam.factory != "All")
            {
                pred = pred.And(x => x.factory_id.Trim() == searchParam.factory.Trim());
            }
            if (!string.IsNullOrEmpty(searchParam.dev_season))
            {
                predModel = predModel.And(x => x.dev_season.ToUpper().Trim().Contains(searchParam.dev_season.ToUpper().Trim()));
            }
            if (!string.IsNullOrEmpty(searchParam.prod_season))
            {
                predModel = predModel.And(x => x.prod_season.ToUpper().Trim().Contains(searchParam.prod_season.ToUpper().Trim()));
            }
            if (!string.IsNullOrEmpty(searchParam.model))
            {
                predModel.And(x => x.model_no.ToUpper().Trim().Contains(searchParam.model.ToUpper().Trim()) || x.model_name.Contains(searchParam.model.ToUpper().Trim()));
            }
            if (!string.IsNullOrEmpty(searchParam.production_adoption))
            {
                pred = pred.And(x => x.production_adoption == Convert.ToBoolean(searchParam.production_adoption.Trim()));
            }

            var dataComputerStitchingSetting = _pBPComputerStitchingSettingRepository.FindAll(pred);
            var dataModel = _modelRepository.FindAll(predModel);

            var data = dataComputerStitchingSetting.Join(dataModel,
                x => new { x.factory_id, x.model_no },
                y => new { y.factory_id, y.model_no },
                (x, y) => new { a = x, b = y })
                .GroupJoin(dataStage,
                    x => new { x.a.factory_id, x.a.stage_id },
                    y => new { y.factory_id, y.stage_id },
                    (x, y) => new { x.a, x.b, c = y })
                    .SelectMany(x => x.c.DefaultIfEmpty(), (x, y) => new { x.a, x.b, c = y })
                    .GroupJoin(dataModelOperation,
                        x => new { x.a.factory_id, x.a.model_no, x.a.stage_id, x.a.operation_id },
                        y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
                        (x, y) => new { x.a, x.b, x.c, d = y })
                        .SelectMany(x => x.d.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, d = y })
                        .GroupJoin(dataCSType,
                            x => new { x.a.factory_id, x.a.cs_type_id },
                            y => new { y.factory_id, y.cs_type_id },
                            (x, y) => new { x.a, x.b, x.c, x.d, e = y })
                            .SelectMany(x => x.e.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, x.d, e = y })
                            .GroupJoin(dataCSMachineType,
                                x => new { x.a.factory_id, x.a.cs_machine_type_id },
                                y => new { y.factory_id, y.cs_machine_type_id },
                                (x, y) => new { x.a, x.b, x.c, x.d, x.e, f = y })
                                .SelectMany(x => x.f.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, x.d, x.e, f = y })
                                .Select(x => new PBP_ComputerStitchingSettingDTO
                                {
                                    factory_id = x.a.factory_id,
                                    model_no = x.a.model_no,
                                    stage_id = x.a.stage_id,
                                    operation_id = x.a.operation_id,
                                    cs_type_id = x.a.cs_type_id,
                                    cs_machine_type_id = x.a.cs_machine_type_id,
                                    dev_season = x.b.dev_season,
                                    production_season = x.b.prod_season,
                                    sop_setup = x.a.sop_setup,
                                    production_adoption = x.a.production_adoption,
                                    model_name = x.b.model_name,
                                    stage_name = x.c.stage_name,
                                    operation_name_zh = x.d.operation_name_zh,
                                    operation_name_en = x.d.operation_name_en,
                                    operation_name_local = x.d.operation_name_local,
                                    is_critical_process = x.a.is_critical_process,
                                    cs_type_name = x.e.cs_type_name,
                                    cs_machine_type_name = x.f.cs_machine_type_name,
                                    cs_speed_setting_rpm = x.a.cs_speed_setting_rpm,
                                    jig_photo_url = x.a.jig_photo_url,
                                    cs_video_url = x.a.cs_video_url,
                                    //
                                    main_upper_material_type_name = dataMainUpper.Where(y => y.main_upper_material_type_id == x.a.main_upper_material_type_id)
                                        .Select(y => y.main_upper_material_type_name).FirstOrDefault(),
                                    cs_machine_model = x.a.cs_machine_model,
                                    number_of_size_group = x.a.number_of_size_group,
                                    jig_design_name = dataJigDesign.Where(y => y.jig_design_id == x.a.jig_design_id)
                                        .Select(y => y.jig_design_name).FirstOrDefault(),
                                    article_no_is_general = x.a.article_no_is_general,
                                    article_no_remarks = x.a.article_no_remarks,
                                    sop_setupResult = x.a.sop_setup ? "YES" : "NO",
                                    production_adoptionResult = x.a.production_adoption ? "YES" : "NO",
                                    article_no_is_generalResult = x.a.article_no_is_general ? "YES" : "NO",
                                    is_critical_processResult = x.a.is_critical_process ? "YES" : "NO",
                                    jig_photo_urlResult = imageUrl + x.a.jig_photo_url,
                                    cs_video_urlResult = imageUrl + x.a.cs_video_url
                                });
            
            data = data.OrderBy(x => x.model_no);
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return await PageListUtility<PBP_ComputerStitchingSettingDTO>.PageListAsync(data, pagination.PageNumber, pagination.PageSize, isPagging);
        }

      
    }
}