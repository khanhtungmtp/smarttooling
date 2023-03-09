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
    public class BondingReportService : IBondingReportService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private readonly IFactoryRepository _factoryRepository;
        private readonly IPBPChemicalProcessTypeRepository _pBPChemicalProcessTypeRepository;
        private readonly IPBPAutoTechTypeRepository _pBPAutoTechTypeRepository;
        private readonly IPBPBondingProgramSettingRepository _pBPBondingProgramSettingRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IPBPProcessAdoptionScopeTypeRepository _pBPProcessAdoptionScopeTypeRepository;
        private readonly IPBPAdoptionComponentTypeRepository _pBPAdoptionComponentTypeRepository;
        private readonly IPBPMainUpperMaterialTypeRepository _pBPMainUpperMaterialTypeRepository;
        private readonly IPBPMainBottomMaterialTypeRepository _pBPMainBottomMaterialTypeRepository;
        private readonly IPBPChemicalSupplierTypeRepository _pBPChemicalSupplierTypeRepository;
        private readonly IImageUrlUtility _imageUrlUtility;

        public BondingReportService(IMapper mapper, MapperConfiguration configMapper,
            IConfiguration configuration, IFactoryRepository factoryRepository,
            IPBPChemicalProcessTypeRepository pBPChemicalProcessTypeRepository,
            IPBPAutoTechTypeRepository pBPAutoTechTypeRepository, IPBPBondingProgramSettingRepository pBPBondingProgramSettingRepository,
            IModelRepository modelRepository, IPBPProcessAdoptionScopeTypeRepository pBPProcessAdoptionScopeTypeRepository,
            IPBPAdoptionComponentTypeRepository pBPAdoptionComponentTypeRepository, IPBPMainUpperMaterialTypeRepository pBPMainUpperMaterialTypeRepository,
            IPBPMainBottomMaterialTypeRepository pBPMainBottomMaterialTypeRepository, IPBPChemicalSupplierTypeRepository pBPChemicalSupplierTypeRepository,
            IImageUrlUtility imageUrlUtility)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _configuration = configuration;
            _factoryRepository = factoryRepository;
            _pBPChemicalProcessTypeRepository = pBPChemicalProcessTypeRepository;
            _pBPAutoTechTypeRepository = pBPAutoTechTypeRepository;
            _pBPBondingProgramSettingRepository = pBPBondingProgramSettingRepository;
            _modelRepository = modelRepository;
            _pBPProcessAdoptionScopeTypeRepository = pBPProcessAdoptionScopeTypeRepository;
            _pBPAdoptionComponentTypeRepository = pBPAdoptionComponentTypeRepository;
            _pBPMainUpperMaterialTypeRepository = pBPMainUpperMaterialTypeRepository;
            _pBPMainBottomMaterialTypeRepository = pBPMainBottomMaterialTypeRepository;
            _pBPChemicalSupplierTypeRepository = pBPChemicalSupplierTypeRepository;
            _imageUrlUtility = imageUrlUtility;
        }

        public async Task<List<KeyValuePair<string, string>>> GetProcessType(string factory_id)
        {
            if (!string.IsNullOrEmpty(factory_id))
                _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
            var data = await _pBPChemicalProcessTypeRepository.FindAll(x => x.is_active == true).OrderBy(x => x.sequence)
                        .Select(x => new KeyValuePair<string, string>(x.chemical_process_type_id.Trim(), x.chemical_process_type_name.Trim()))
                        .Distinct().ToListAsync();
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return data;
        }
        public async Task<List<KeyValuePair<string, string>>> GetAllFactory()
        {
            var data = await _factoryRepository.FindAll().Select(x => new KeyValuePair<string, string>(x.factory_id.Trim(), x.factory_name.Trim())).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAutoTech(string factory_id)
        {
            if (!string.IsNullOrEmpty(factory_id))
                _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
            var data = await _pBPAutoTechTypeRepository.FindAll(x => x.is_active == true).OrderBy(x => x.sequence)
                        .Select(x => new KeyValuePair<string, string>(x.auto_tech_id.Trim(), x.auto_tech_name.Trim()))
                        .Distinct().ToListAsync();
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return data;
        }

        public async Task<PageListUtility<PBP_Bonding_Program_SettingDTO>> Search(PaginationParams pagination, BondingReportParam searchParam, bool isPaging)
        {
            List<PBP_Bonding_Program_SettingDTO> result = new List<PBP_Bonding_Program_SettingDTO>();
            if (!string.IsNullOrEmpty(searchParam.factory?.Trim()))
            {
                var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(searchParam.factory);
                result = await SearchDataEachFactory(searchParam, searchParam.factory, imageUrl);
            }
            else
            {
                string area = _configuration.GetSection("AppSettings:Area").Value;
                List<string> FactoryList = new List<string> { "SHC", "CB", "TSH", "SPC" };
                foreach (var item in FactoryList)
                {
                    var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(item);
                    var data = await SearchDataEachFactory(searchParam, item, imageUrl);
                    result.AddRange(data);
                }
        
            }
            return PageListUtility<PBP_Bonding_Program_SettingDTO>.PageList(result, pagination.PageNumber, pagination.PageSize, isPaging);
        }

        private async Task<List<PBP_Bonding_Program_SettingDTO>> SearchDataEachFactory(BondingReportParam searchParam, string factory, string imageUrl)
        {
            var pred = PredicateBuilder.New<PBP_Bonding_Program_Setting>(true);
            var predModel = PredicateBuilder.New<Model>(true);
            _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
            pred = pred.And(x => x.factory_id.Trim() == factory.Trim());
            if (!string.IsNullOrEmpty(searchParam.chemical_process_type_id?.Trim()))
            {
                pred = pred.And(x => x.chemical_process_type_id.Trim() == searchParam.chemical_process_type_id.Trim());
            }
            if (!string.IsNullOrEmpty(searchParam.auto_tech_id?.Trim()))
            {
                pred = pred.And(x => x.auto_tech_id.Trim() == searchParam.auto_tech_id.Trim());
            }
            if(!string.IsNullOrEmpty(searchParam.model?.Trim())) {
                predModel.And(x => x.model_no.ToUpper().Trim().Contains(searchParam.model.ToUpper().Trim()) || x.model_name.Contains(searchParam.model.ToUpper().Trim()));
            }
            if (!string.IsNullOrEmpty(searchParam.prod_season?.Trim()))
            {
                predModel = predModel.And(x => x.prod_season.ToUpper().Trim().Contains(searchParam.prod_season.ToUpper().Trim()));
            }
            var dataBondingProgramSetting = _pBPBondingProgramSettingRepository.FindAll(pred);
            var dataModel = _modelRepository.FindAll(predModel);
            var dataChemicalProcessType = _pBPChemicalProcessTypeRepository.FindAll();
            var dataAutoTechType = _pBPAutoTechTypeRepository.FindAll();
            var dataAdoptionComponent = _pBPAdoptionComponentTypeRepository.FindAll();
            var dataChemicalSupplierType = _pBPChemicalSupplierTypeRepository.FindAll();
            var dataProcessAdoptionScope = _pBPProcessAdoptionScopeTypeRepository.FindAll();
            var dataUpperMaterial = _pBPMainUpperMaterialTypeRepository.FindAll();
            var dataBottomMaterial = _pBPMainBottomMaterialTypeRepository.FindAll();

            var data = dataBondingProgramSetting.Join(
                dataModel,
                x => new { x.factory_id, x.model_no },
                y => new { y.factory_id, y.model_no },
                (x,y) => new {BondingProgramSetting = x, Model = y}
            ).GroupJoin(dataChemicalProcessType,
                    x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.chemical_process_type_id },
                    y => new { y.factory_id, y.chemical_process_type_id },
                    (x, y) => new { x.BondingProgramSetting, x.Model, ChemicalProcessType = y })
                    .SelectMany(x => x.ChemicalProcessType.DefaultIfEmpty(),
                    (x, y) => new { x.BondingProgramSetting, x.Model, ChemicalProcessType = y })
                    .GroupJoin(dataAutoTechType,
                        x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.auto_tech_id },
                        y => new { y.factory_id, y.auto_tech_id },
                        (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, AutoTechType = y })
                        .SelectMany(x => x.AutoTechType.DefaultIfEmpty(),
                        (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, AutoTechType = y })
                        .GroupJoin(dataAdoptionComponent,
                            x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.adoption_component_id },
                            y => new { y.factory_id, y.adoption_component_id },
                            (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, AdoptionComponent = y })
                            .SelectMany(x => x.AdoptionComponent.DefaultIfEmpty(),
                            (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, AdoptionComponent = y })
                            .GroupJoin(dataChemicalSupplierType,
                                x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.chemical_supplier_id },
                                y => new { y.factory_id, y.chemical_supplier_id },
                                (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, x.AdoptionComponent, ChemicalSupplierType = y })
                                .SelectMany(x => x.ChemicalSupplierType.DefaultIfEmpty(),
                                (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, x.AdoptionComponent, ChemicalSupplierType = y })
                                .GroupJoin(dataProcessAdoptionScope,
                                    x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.process_adoption_scope_id },
                                    y => new { y.factory_id, y.process_adoption_scope_id },
                                    (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, x.AdoptionComponent, x.ChemicalSupplierType, ProcessAdoptionScope = y })
                                    .SelectMany(x => x.ProcessAdoptionScope.DefaultIfEmpty(),
                                    (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, x.AdoptionComponent, x.ChemicalSupplierType, ProcessAdoptionScope = y })
                                    .GroupJoin(dataUpperMaterial,
                                        x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.main_upper_material_type_id },
                                        y => new { y.factory_id, y.main_upper_material_type_id },
                                        (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, x.AdoptionComponent, x.ChemicalSupplierType, x.ProcessAdoptionScope, UpperMaterial = y })
                                        .SelectMany(x => x.UpperMaterial.DefaultIfEmpty(),
                                        (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, x.AdoptionComponent, x.ChemicalSupplierType, x.ProcessAdoptionScope, UpperMaterial = y })
                                        .GroupJoin(dataBottomMaterial,
                                            x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.main_bottom_material_type_id },
                                            y => new { y.factory_id, y.main_bottom_material_type_id },
                                            (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, x.AdoptionComponent, x.ChemicalSupplierType, x.ProcessAdoptionScope, x.UpperMaterial, BottomMaterial = y })
                                            .SelectMany(x => x.BottomMaterial.DefaultIfEmpty(),
                                            (x, y) => new { x.BondingProgramSetting, x.ChemicalProcessType, x.Model, x.AutoTechType, x.AdoptionComponent, x.ChemicalSupplierType, x.ProcessAdoptionScope, x.UpperMaterial, BottomMaterial = y })
                                            .Select(x => new PBP_Bonding_Program_SettingDTO
                                            {
                                                factory_id = x.BondingProgramSetting.factory_id,
                                                dev_season = x.Model.dev_season,
                                                production_season = x.Model.prod_season,
                                                model_no = x.BondingProgramSetting.model_no,
                                                model_name = x.Model.model_name,
                                                chemical_process_type_id = x.BondingProgramSetting.chemical_process_type_id,
                                                chemical_process_type_name = x.ChemicalProcessType.chemical_process_type_name,
                                                auto_tech_id = x.BondingProgramSetting.auto_tech_id,
                                                auto_tech_name = x.AutoTechType.auto_tech_name,
                                                chemical_name = x.BondingProgramSetting.chemical_name,
                                                adoption_component_id = x.BondingProgramSetting.adoption_component_id,
                                                adoption_component_name = x.AdoptionComponent.adoption_component_name,
                                                chemical_supplier_id = x.BondingProgramSetting.chemical_supplier_id,
                                                chemical_supplier_name = x.ChemicalSupplierType.chemical_supplier_name,
                                                process_adoption_scope_id = x.BondingProgramSetting.process_adoption_scope_id,
                                                process_adoption_scope_name = x.ProcessAdoptionScope.process_adoption_scope_name,
                                                first_month_of_production_adoption = x.BondingProgramSetting.first_month_of_production_adoption,
                                                main_upper_material_type_name = x.UpperMaterial.main_upper_material_type_name,
                                                main_bottom_material_type_name = x.BottomMaterial.main_bottom_material_type_name,
                                                article_no_is_general = x.BondingProgramSetting.article_no_is_general,
                                                article_no_remarks = x.BondingProgramSetting.article_no_remarks,
                                                shoes_photo_url = x.BondingProgramSetting.shoes_photo_url,
                                                article_no_is_general_result = x.BondingProgramSetting.article_no_is_general ? "YES" : "NO",
                                                shoes_photo_url_result = imageUrl + x.BondingProgramSetting.shoes_photo_url
                                            });

            // if (!string.IsNullOrEmpty(searchParam.model?.Trim()))
            // {
            //     data = data.Where(x => x.model_no.ToUpper().Trim().Contains(searchParam.model.ToUpper().Trim()) || x.model_name.Contains(searchParam.model.ToUpper().Trim()));
            // }
            data = data.OrderBy(x => x.model_no);
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return await data.ToListAsync();
        }
    }
}