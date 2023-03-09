using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API.Models.ProductionBP;
using SmartTooling_API.DTO.ProductionBP.BondingProgramSetting;
using SmartTooling_API.Helpers.Params.ProductionBP;
using LinqKit;
using AutoMapper;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Models.SmartTool;
using Microsoft.AspNetCore.Http;
using Aspose.Cells;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace SmartTooling_API._Services.Services.ProductionBP
{
    public class PBPBondingProgramSettingService : IPBPBondingProgramSettingService
    {
        private readonly IFactoryRepository _factoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IPBPBondingProgramSettingRepository _pBPBondingProgramSettingRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IPBPAutoTechTypeRepository _pBPAutoTechTypeRepository;
        private readonly IPBPChemicalProcessTypeRepository _pBPChemicalProcessTypeRepository;
        private readonly IPBPProcessAdoptionScopeTypeRepository _pBPProcessAdoptionScopeTypeRepository;
        private readonly IPBPAdoptionComponentTypeRepository _pBPAdoptionComponentTypeRepository;
        private readonly IPBPMainUpperMaterialTypeRepository _pBPMainUpperMaterialTypeRepository;
        private readonly IPBPMainBottomMaterialTypeRepository _pBPMainBottomMaterialTypeRepository;
        private readonly IPBPChemicalSupplierTypeRepository _pBPChemicalSupplierTypeRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnv;

        public PBPBondingProgramSettingService(
            IFactoryRepository factoryRepository,
            IPBPBondingProgramSettingRepository pBPBondingProgramSettingRepository,
            IModelRepository modelRepository,
            IPBPAutoTechTypeRepository pBPAutoTechTypeRepository,
            IPBPChemicalProcessTypeRepository pBPChemicalProcessTypeRepository,
            IPBPProcessAdoptionScopeTypeRepository pBPProcessAdoptionScopeTypeRepository,
            IPBPAdoptionComponentTypeRepository pBPAdoptionComponentTypeRepository,
            IPBPMainUpperMaterialTypeRepository pBPMainUpperMaterialTypeRepository,
            IPBPMainBottomMaterialTypeRepository pBPMainBottomMaterialTypeRepository,
            IPBPChemicalSupplierTypeRepository pBPChemicalSupplierTypeRepository,
            IConfiguration configuration,
            IWebHostEnvironment webHostEnv,
            IMapper mapper)
        {
            _mapper = mapper;
            _factoryRepository = factoryRepository;
            _configuration = configuration;
            _pBPBondingProgramSettingRepository = pBPBondingProgramSettingRepository;
            _modelRepository = modelRepository;
            _pBPAutoTechTypeRepository = pBPAutoTechTypeRepository;
            _pBPChemicalProcessTypeRepository = pBPChemicalProcessTypeRepository;
            _pBPProcessAdoptionScopeTypeRepository = pBPProcessAdoptionScopeTypeRepository;
            _pBPAdoptionComponentTypeRepository = pBPAdoptionComponentTypeRepository;
            _pBPMainUpperMaterialTypeRepository = pBPMainUpperMaterialTypeRepository;
            _pBPChemicalSupplierTypeRepository = pBPChemicalSupplierTypeRepository;
            _pBPMainBottomMaterialTypeRepository = pBPMainBottomMaterialTypeRepository;
            _webHostEnv = webHostEnv;
        }

        public async Task<bool> AddBondingProgramSetting(PBP_Bonding_Program_SettingDTO model)
        {
            if (await IsExists(model))
            {
                return false;
            }
            else
            {
                var models = _mapper.Map<PBP_Bonding_Program_Setting>(model);
                _pBPBondingProgramSettingRepository.Add(models);
                return await _pBPBondingProgramSettingRepository.SaveAll();
            }
        }

        public async Task<bool> UpdateBondingProgramSetting(PBP_Bonding_Program_SettingDTO model)
        {
            var result = await _pBPBondingProgramSettingRepository
                .FindAll(x =>
                    x.factory_id == model.factory_id &&
                    x.model_no == model.model_no &&
                    x.chemical_process_type_id == model.chemical_process_type_id &&
                    x.auto_tech_id == model.auto_tech_id &&
                    x.chemical_name == model.chemical_name &&
                    x.adoption_component_id == model.adoption_component_id)
                .FirstOrDefaultAsync();

            // ==== Don't allow to edit ====
            //result.factory_id = model.factory_id;
            //result.model_no = model.model_no;
            //result.chemical_process_type_id = model.chemical_process_type_id;
            //result.auto_tech_id = model.auto_tech_id;
            //result.chemical_name = model.chemical_name;
            //result.adoption_component_id = model.adoption_component_id;

            result.chemical_supplier_id = model.chemical_supplier_id;
            result.process_adoption_scope_id = model.process_adoption_scope_id;
            result.first_month_of_production_adoption = model.first_month_of_production_adoption;
            result.main_upper_material_type_id = model.main_upper_material_type_id;
            result.main_bottom_material_type_id = model.main_bottom_material_type_id;
            result.article_no_is_general = model.article_no_is_general;
            result.article_no_remarks = model.article_no_remarks;
            if(!model.shoes_photo_url.Contains("http")) {
                 result.shoes_photo_url = model.shoes_photo_url;
            }
           
            result.update_by = model.update_by;
            result.update_time = model.update_time;

            return await _pBPBondingProgramSettingRepository.SaveAll();

        }

        public async Task<List<KeyValuePair<string, string>>> GetAllAutoTech()
        {
            var autoTechType = _pBPAutoTechTypeRepository.FindAll(x => x.is_active == true);
            var data = await autoTechType
                .Select(m => new KeyValuePair<string, string>(
                    m.auto_tech_id,
                    m.auto_tech_name))
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllChemicalProcessType()
        {
            var chemicalProcessType = _pBPChemicalProcessTypeRepository.FindAll(x => x.is_active == true);
            var data = await chemicalProcessType
                .Select(m => new KeyValuePair<string, string>(
                    m.chemical_process_type_id,
                    m.chemical_process_type_name))
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllModelNo()
        {
            var data = await _modelRepository
                .FindAll(x => x.is_active == true)
                .Select(x => new KeyValuePair<string, string>(
                    x.model_no.Trim(),
                    x.model_name.Trim()))
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllAdoptionComponentType()
        {
            var data = await _pBPAdoptionComponentTypeRepository
                .FindAll(x => x.is_active == true)
                .Select(x => new KeyValuePair<string, string>(
                    x.adoption_component_id,
                    x.adoption_component_name))
                .Distinct()
                .ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllProcessAdoptionScopeType()
        {
            var data = await _pBPProcessAdoptionScopeTypeRepository
                .FindAll(x => x.is_active == true)
                .Select(x => new KeyValuePair<string, string>(
                    x.process_adoption_scope_id.Trim(),
                    x.process_adoption_scope_name.Trim()))
                .Distinct()
                .ToListAsync();

            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllMainUpperMaterialType()
        {
            var data = await _pBPMainUpperMaterialTypeRepository
                .FindAll(x => x.is_active == true)
                .Select(x => new KeyValuePair<string, string>(
                    x.main_upper_material_type_id.Trim(),
                    x.main_upper_material_type_name.Trim()))
                .Distinct()
                .ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllMainBottomMaterialType()
        {
            var data = await _pBPMainBottomMaterialTypeRepository
                .FindAll(x => x.is_active == true)
                .Select(x => new KeyValuePair<string, string>(
                    x.main_bottom_material_type_id.Trim(),
                    x.main_bottom_material_type_name.Trim()))
                .Distinct()
                .ToListAsync();
            return data;
        }


        public async Task<bool> IsExists(PBP_Bonding_Program_SettingDTO model)
        {
            var item = await _pBPBondingProgramSettingRepository
                .FindAll(
                    x => x.factory_id == model.factory_id &&
                    x.model_no == model.model_no &&
                    x.chemical_process_type_id == model.chemical_process_type_id &&
                    x.auto_tech_id == model.auto_tech_id &&
                    x.chemical_name == model.chemical_name &&
                    x.adoption_component_id == model.adoption_component_id)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return item == null ? false : true;
        }

        public async Task<PagedList<BondingProgramSettingModelDTO>> Search(PaginationParams pagination, BondingProgramSettingParam filterParam)
        {
            var layoutPred = PredicateBuilder.New<PBP_Bonding_Program_Setting>(true);
            var predModel = PredicateBuilder.New<Model>(true);

            if (!string.IsNullOrEmpty(filterParam.chemical_process_type_id))
            {
                layoutPred = layoutPred.And(x => x.chemical_process_type_id.Trim() == filterParam.chemical_process_type_id.Trim());
            }
            if (!string.IsNullOrEmpty(filterParam.auto_tech_id))
            {
                layoutPred = layoutPred.And(x => x.auto_tech_id.Trim() == filterParam.auto_tech_id.Trim());
            }
            if (!string.IsNullOrEmpty(filterParam.production_season))
            {
                predModel = predModel.And(x => x.prod_season.Trim() == filterParam.production_season.Trim());
            }

            var bondingProgramSetting = _pBPBondingProgramSettingRepository.FindAll(layoutPred);
            var model = _modelRepository.FindAll(predModel);
            var chemicalProcessType = _pBPChemicalProcessTypeRepository.FindAll();
            var autoTechType = _pBPAutoTechTypeRepository.FindAll();
            var processAdoptionScopeType = _pBPProcessAdoptionScopeTypeRepository.FindAll();
            var adoptionComponentType = _pBPAdoptionComponentTypeRepository.FindAll();

            var resultList = bondingProgramSetting.Join(model,
                x => new { x.factory_id, x.model_no },
                y => new { y.factory_id, y.model_no },
                (x, y) => new { BondingProgramSetting = x, Model = y })
            .GroupJoin(chemicalProcessType,
                x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.chemical_process_type_id },
                y => new { y.factory_id, y.chemical_process_type_id },
                (x, y) => new { x.BondingProgramSetting, x.Model, ChemicalProcessType = y })
            .SelectMany(
                x => x.ChemicalProcessType.DefaultIfEmpty(),
                (x, y) => new { x.BondingProgramSetting, x.Model, ChemicalProcessType = y })
            .GroupJoin(autoTechType,
                x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.auto_tech_id },
                y => new { y.factory_id, y.auto_tech_id },
                (x, y) => new { x.BondingProgramSetting, x.Model, x.ChemicalProcessType, AutoTechType = y })
            .SelectMany(
                x => x.AutoTechType.DefaultIfEmpty(),
                (x, y) => new { x.BondingProgramSetting, x.Model, x.ChemicalProcessType, AutoTechType = y })
            .GroupJoin(processAdoptionScopeType,
                x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.process_adoption_scope_id },
                y => new { y.factory_id, y.process_adoption_scope_id },
                (x, y) => new { x.BondingProgramSetting, x.Model, x.ChemicalProcessType, x.AutoTechType, ProcessAdoptionScopeType = y })
            .SelectMany(
                x => x.ProcessAdoptionScopeType.DefaultIfEmpty(),
                (x, y) => new { x.BondingProgramSetting, x.Model, x.ChemicalProcessType, x.AutoTechType, ProcessAdoptionScopeType = y })
            .GroupJoin(adoptionComponentType,
                x => new { x.BondingProgramSetting.factory_id, x.BondingProgramSetting.adoption_component_id },
                y => new { y.factory_id, y.adoption_component_id },
                (x, y) => new { x.BondingProgramSetting, x.Model, x.ChemicalProcessType, x.AutoTechType, x.ProcessAdoptionScopeType, AdoptionComponentType = y })
            .SelectMany(
                x => x.AdoptionComponentType.DefaultIfEmpty(),
                (x, y) => new BondingProgramSettingModelDTO
                {
                    factory_id = x.BondingProgramSetting.factory_id,
                    production_season = x.Model.prod_season,
                    dev_season = x.Model.dev_season,
                    model_no = x.BondingProgramSetting.model_no,
                    model_name = x.Model.model_name,
                    chemical_process_type_id = x.BondingProgramSetting.chemical_process_type_id,
                    chemical_process_type_name = x.ChemicalProcessType.chemical_process_type_name,
                    auto_tech_id = x.BondingProgramSetting.auto_tech_id,
                    auto_tech_name = x.AutoTechType.auto_tech_name,
                    chemical_name = x.BondingProgramSetting.chemical_name,
                    chemical_supplier_id = x.BondingProgramSetting.chemical_supplier_id,
                    first_month_of_production_adoption = x.BondingProgramSetting.first_month_of_production_adoption,
                    process_adoption_scope_id = x.BondingProgramSetting.process_adoption_scope_id,
                    process_adoption_scope_name = x.ProcessAdoptionScopeType.process_adoption_scope_name,
                    adoption_component_id = x.BondingProgramSetting.adoption_component_id,
                    adoption_component_name = y.adoption_component_name,
                    main_upper_material_type_id = x.BondingProgramSetting.main_upper_material_type_id,
                    main_bottom_material_type_id = x.BondingProgramSetting.main_bottom_material_type_id,
                    article_no_is_general = x.BondingProgramSetting.article_no_is_general,
                    article_no_remarks = x.BondingProgramSetting.article_no_remarks,
                    shoes_photo_url = x.BondingProgramSetting.shoes_photo_url,
                    create_by = x.BondingProgramSetting.create_by,
                    create_time = x.BondingProgramSetting.create_time,
                    update_by = x.BondingProgramSetting.update_by,
                    update_time = x.BondingProgramSetting.update_time
                });

            if (!string.IsNullOrEmpty(filterParam.model))
            {
                resultList = resultList.Where(x => x.model_no.ToUpper().Contains(filterParam.model.ToUpper()) || x.model_name.ToUpper().Contains(filterParam.model.ToUpper()));
            }


            return await PagedList<BondingProgramSettingModelDTO>.CreateAsync(resultList, pagination.PageNumber, pagination.PageSize);
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllChemicalSupplier()
        {
            var data = await _pBPChemicalSupplierTypeRepository
                .FindAll(x => x.is_active == true)
                .Select(x => new KeyValuePair<string, string>(
                    x.chemical_supplier_id.Trim(),
                    x.chemical_supplier_name.Trim()))
                .Distinct()
                .ToListAsync();
            return data;
        }

        public async Task<OperationResult> UploadExcel(IFormFile file, string user)
        {
            var factory = _configuration.GetSection("AppSettings:Factory").Value;
            //Lưu file vào wwwroot
            if (file == null)
            {
                return new OperationResult(false, "File not found.");
            }
            var extension = Path.GetExtension(file.FileName).ToLower();
            var uploadFile = $"Sample_BondingProgramSetting{extension}";
            string uploadPath = @"uploaded/excels";
            string folder = Path.Combine(_webHostEnv.WebRootPath, uploadPath);

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            string filePath = Path.Combine(folder, uploadFile);
            if (File.Exists(filePath))
                File.Delete(filePath);
            try
            {
                using (FileStream fs = File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            //Đọc file
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(filePath);
            Worksheet ws = designer.Workbook.Worksheets[0];

            int rows = ws.Cells.MaxDataRow;
            if (rows < 1)
                return new OperationResult(false, "An empty excel file", "Error");

            var pBPBondingProgramSettingList = new List<PBP_Bonding_Program_Setting>();

            for (int i = 1; i <= rows; i++)
            {
                var modelNo = ws.Cells[i, 0].StringValue.Trim();
                if (!(_modelRepository.FindAll(x => x.is_active == true && x.model_no.Trim() == modelNo).AsNoTracking().Any()))
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    return new OperationResult(false, "", "NotModel", modelNo);
                }

                var chemicalProcessTypeId = ws.Cells[i, 1].StringValue.Trim();
                var autoTechType = ws.Cells[i, 2].StringValue.Trim();
                var chemicalName = ws.Cells[i, 3].StringValue.Trim();
                var adoptionComponentId = ws.Cells[i, 4].StringValue.Trim();
                var chemicalSupplierId = ws.Cells[i, 5].StringValue.Trim();
                var ArticleNogeneral = ws.Cells[i, 6].StringValue.Trim();
                var ProcessAdoptionScopeTypeId = ws.Cells[i, 8].StringValue.Trim();
                var mainUpperMaterialTypeId = ws.Cells[i, 9].StringValue.Trim();
                var mainBottomMaterialTypeId = ws.Cells[i, 10].StringValue.Trim();
                var firtMonthOfProduction = ws.Cells[i, 11].DateTimeValue;

                var model = await _pBPBondingProgramSettingRepository.FindAll(x =>
                x.factory_id == factory &&
                x.model_no == modelNo &&
                x.chemical_process_type_id == chemicalProcessTypeId &&
                x.auto_tech_id == autoTechType &&
                x.chemical_name == chemicalName &&
                x.adoption_component_id == adoptionComponentId).FirstOrDefaultAsync();

                var modelN = await _modelRepository.FindAll(x => x.model_no == modelNo).FirstOrDefaultAsync();

                var chemicalSupplier = await _pBPChemicalSupplierTypeRepository.FindAll(x => x.chemical_supplier_id == chemicalSupplierId).FirstOrDefaultAsync();

                var adoptionComponentType = await _pBPAdoptionComponentTypeRepository.FindAll(x => x.adoption_component_id == adoptionComponentId).FirstOrDefaultAsync();

                var processAdoptionScopeType = await _pBPProcessAdoptionScopeTypeRepository.FindAll(x => x.process_adoption_scope_id == ProcessAdoptionScopeTypeId).FirstOrDefaultAsync();

                var mainBottomMaterialType = await _pBPMainBottomMaterialTypeRepository.FindAll(x => x.main_bottom_material_type_id == mainBottomMaterialTypeId).FirstOrDefaultAsync();

                var mainUpperMaterialType = await _pBPMainUpperMaterialTypeRepository.FindAll(x => x.main_upper_material_type_id == mainUpperMaterialTypeId).FirstOrDefaultAsync();

                Regex regex = new Regex(@"^[A-Za-z ]+$");
                if (model == null)
                {
                    if (regex.IsMatch(chemicalName) &&
                        !string.IsNullOrEmpty(modelNo) &&
                        !string.IsNullOrEmpty(chemicalProcessTypeId) &&
                        !string.IsNullOrEmpty(autoTechType) &&
                        !string.IsNullOrEmpty(chemicalName) &&
                        !string.IsNullOrEmpty(adoptionComponentId) &&
                        !string.IsNullOrEmpty(chemicalSupplierId) &&
                        !string.IsNullOrEmpty(ArticleNogeneral) &&
                        !string.IsNullOrEmpty(ProcessAdoptionScopeTypeId) &&
                        !string.IsNullOrEmpty(mainBottomMaterialTypeId) &&
                        !string.IsNullOrEmpty(mainUpperMaterialTypeId) &&
                        !string.IsNullOrEmpty(firtMonthOfProduction.ToString()))
                    {
                        var bondingProgramSetting = new PBP_Bonding_Program_Setting();
                        if (!string.IsNullOrEmpty(chemicalName) && chemicalSupplier != null &&
                        chemicalSupplier != null && processAdoptionScopeType != null && mainBottomMaterialType != null && mainUpperMaterialType != null &&
                        adoptionComponentType != null && modelN != null)
                        {
                            bondingProgramSetting.factory_id = factory;
                            bondingProgramSetting.model_no = modelNo;
                            bondingProgramSetting.chemical_process_type_id = chemicalProcessTypeId;
                            bondingProgramSetting.auto_tech_id = autoTechType;
                            bondingProgramSetting.chemical_name = chemicalName;
                            bondingProgramSetting.chemical_supplier_id = chemicalSupplierId;
                            bondingProgramSetting.process_adoption_scope_id = ProcessAdoptionScopeTypeId;
                            bondingProgramSetting.main_bottom_material_type_id = mainBottomMaterialTypeId;
                            bondingProgramSetting.main_upper_material_type_id = mainUpperMaterialTypeId;
                            bondingProgramSetting.adoption_component_id = adoptionComponentId;
                            bondingProgramSetting.article_no_is_general = ArticleNogeneral == "Yes" ? true : false;
                            if (ArticleNogeneral == "Yes")
                            {
                                bondingProgramSetting.article_no_remarks = null;
                            }
                            else
                            {
                                bondingProgramSetting.article_no_remarks = ws.Cells[i, 7].StringValue;
                            }

                            bondingProgramSetting.first_month_of_production_adoption = firtMonthOfProduction;
                            bondingProgramSetting.shoes_photo_url = factory + "/no-image.jpg";
                            bondingProgramSetting.create_by = user;
                            bondingProgramSetting.create_time = DateTime.Now;
                            bondingProgramSetting.update_by = user;
                            bondingProgramSetting.update_time = DateTime.Now;
                        }
                        else
                        {
                            continue;
                        }

                        var item = _mapper.Map<PBP_Bonding_Program_Setting>(bondingProgramSetting);
                        pBPBondingProgramSettingList.Add(item);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    continue;
                }
            }

            _pBPBondingProgramSettingRepository.AddMultiple(pBPBondingProgramSettingList);
            if (await _pBPAdoptionComponentTypeRepository.SaveAll())
            {
                return new OperationResult(true, "File was uploaded.", "Success!");
            }
            else
            {
                return new OperationResult(false, "Uploading file failed on save. Please check the excel data again", "Error!");
            }
        }
    }
}
