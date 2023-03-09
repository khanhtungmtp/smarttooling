using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Aspose.Cells;
using AutoMapper;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.ProductionBP;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Helpers.Params.ProductionBP;
using SmartTooling_API.Models.ProductionBP;

namespace SmartTooling_API._Services.Services.ProductionBP
{
    public class BPComputerStitchingSettingService : IBPComputerStitchingSettingService
    {
        private readonly IPBPComputerStitchingSettingRepository _computerRepo;
        private readonly IModelRepository _modelRepo;
        private readonly IModelOperationRepository _modelOperationRepo;
        private readonly IStageRepository _stageRepo;
        private readonly IPBPCSTypeRepository _cSTypeRepo;
        private readonly IPBPCSMachineTypeRepository _cSMachineTypeRepo;
        private readonly IPBPMainUpperMaterialTypeRepository _mainUpperMaterialRepo;
        private readonly IPBPJigDesignTypeRepository _jigDesignTypeRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public BPComputerStitchingSettingService(IPBPComputerStitchingSettingRepository computerRepo, IModelRepository modelRepo, IModelOperationRepository modelOperationRepo, IStageRepository stageRepo, IPBPCSTypeRepository cSTypeRepo, IPBPCSMachineTypeRepository cSMachineTypeRepo, IPBPMainUpperMaterialTypeRepository mainUpperMaterialRepo, IPBPJigDesignTypeRepository jigDesignTypeRepo, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _computerRepo = computerRepo;
            _modelRepo = modelRepo;
            _modelOperationRepo = modelOperationRepo;
            _stageRepo = stageRepo;
            _cSTypeRepo = cSTypeRepo;
            _cSMachineTypeRepo = cSMachineTypeRepo;
            _mainUpperMaterialRepo = mainUpperMaterialRepo;
            _jigDesignTypeRepo = jigDesignTypeRepo;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task<PagedList<ComputerStitchingSettingViewDTO>> GetAllComputerStitchingSetting(BPSearchComputerStitchingSetting search, PaginationParams pagination)
        {
            var predComputer = PredicateBuilder.New<PBP_ComputerStitchingSetting>(true);
            if (!string.IsNullOrEmpty(search.ProductionAdoption))
            {
                predComputer = predComputer.And(x => x.production_adoption == Convert.ToBoolean(search.ProductionAdoption.Trim()));
            }

            var pBPComputerQuery = _computerRepo.FindAll(predComputer);

            var modelQuery = _modelRepo.FindAll().Select(x => new
            {
                x.factory_id,
                x.model_no,
                x.model_name,
                x.dev_season,
                x.prod_season
            });

            var stageQuery = _stageRepo.FindAll().Select(x => new
            {
                x.factory_id,
                x.stage_id,
                x.stage_name
            });

            var modelOperationQuery = _modelOperationRepo.FindAll().Select(x => new
            {
                x.factory_id,
                x.model_no,
                x.stage_id,
                x.operation_id,
                x.operation_name_zh,
                x.operation_name_en,
                x.operation_name_local,
            });

            var pBPCSTypeQuery = _cSTypeRepo.FindAll().Select(x => new
            {
                x.factory_id,
                x.cs_type_id,
                x.cs_type_name,
            });

            var pBPCSMachineTypeQuery = _cSMachineTypeRepo.FindAll().Select(x => new
            {
                x.factory_id,
                x.cs_machine_type_id,
                x.cs_machine_type_name,
            });

            var jigDesignTypeQuery = _jigDesignTypeRepo.FindAll().Select(x => new
            {
                x.factory_id,
                x.jig_design_id,
                x.jig_design_name
            });

            var queryJoin = pBPComputerQuery.GroupJoin(modelQuery,
                    x => new { x.factory_id, x.model_no },
                    y => new { y.factory_id, y.model_no },
                    (x, y) => new { PBPComputer = x, Models = y })
                .SelectMany(s => s.Models.DefaultIfEmpty(), (x, y) => new { x.PBPComputer, Model = y })
                .GroupJoin(stageQuery,
                    x => new { x.PBPComputer.factory_id, x.PBPComputer.stage_id },
                    y => new { y.factory_id, y.stage_id },
                    (x, y) => new { x.PBPComputer, x.Model, Stages = y })
                .SelectMany(s => s.Stages.DefaultIfEmpty(), (x, y) => new { x.PBPComputer, x.Model, Stage = y })
                .GroupJoin(modelOperationQuery,
                    x => new { x.PBPComputer.factory_id, x.PBPComputer.model_no, x.PBPComputer.stage_id, x.PBPComputer.operation_id },
                    y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
                    (x, y) => new { x.PBPComputer, x.Model, x.Stage, ModelOperations = y })
                .SelectMany(s => s.ModelOperations.DefaultIfEmpty(), (x, y) => new
                {
                    x.PBPComputer,
                    x.Model,
                    x.Stage,
                    ModelOperation = y
                })
                .GroupJoin(pBPCSTypeQuery,
                    x => new { x.PBPComputer.factory_id, x.PBPComputer.cs_type_id },
                    y => new { y.factory_id, y.cs_type_id },
                    (x, y) => new { x.PBPComputer, x.Model, x.Stage, x.ModelOperation, CSTypes = y })
                .SelectMany(s => s.CSTypes.DefaultIfEmpty(), (x, y) => new
                {
                    x.PBPComputer,
                    x.Model,
                    x.Stage,
                    x.ModelOperation,
                    CSType = y
                })
                .GroupJoin(pBPCSMachineTypeQuery,
                    x => new { x.PBPComputer.factory_id, x.PBPComputer.cs_machine_type_id },
                    y => new { y.factory_id, y.cs_machine_type_id },
                    (x, y) => new { x.PBPComputer, x.Model, x.Stage, x.ModelOperation, x.CSType, CSMachineTypes = y })
                .SelectMany(s => s.CSMachineTypes.DefaultIfEmpty(), (x, y) => new
                {
                    x.PBPComputer,
                    x.Model,
                    x.Stage,
                    x.ModelOperation,
                    x.CSType,
                    CSMachineType = y
                })
                .GroupJoin(jigDesignTypeQuery,
                    x => new { x.PBPComputer.factory_id, x.PBPComputer.jig_design_id },
                    y => new { y.factory_id, y.jig_design_id },
                    (x, y) => new { x.PBPComputer, x.Model, x.Stage, x.ModelOperation, x.CSType, x.CSMachineType, JigDesignType = y })
                .SelectMany(s => s.JigDesignType.DefaultIfEmpty(), (x, y) => new ComputerStitchingSettingViewDTO
                {
                    factory_id = x.PBPComputer.factory_id,
                    dev_season = x.Model.dev_season,
                    production_season = x.Model.prod_season,
                    model_no = x.PBPComputer.model_no,
                    model_name = x.Model.model_name,
                    stage_id = x.PBPComputer.stage_id,
                    stage_name = x.Stage.stage_name,
                    operation_id = x.ModelOperation.operation_id,
                    operation_name_en = x.ModelOperation.operation_name_en,
                    operation_name_local = x.ModelOperation.operation_name_local,
                    operation_name_zh = x.ModelOperation.operation_name_zh,
                    sop_setup = x.PBPComputer.sop_setup,
                    production_adoption = x.PBPComputer.production_adoption,
                    is_critical_process = x.PBPComputer.is_critical_process,
                    cs_type_id = x.CSType.cs_type_id,
                    cs_type_name = x.CSType.cs_type_name,
                    cs_machine_type_id = x.CSMachineType.cs_machine_type_id,
                    cs_machine_type_name = x.CSMachineType.cs_machine_type_name,
                    cs_speed_setting_rpm = x.PBPComputer.cs_speed_setting_rpm,
                    jig_photo_url = x.PBPComputer.jig_photo_url,
                    cs_video_url = x.PBPComputer.cs_video_url,
                    main_upper_material_type_id = x.PBPComputer.main_upper_material_type_id,
                    article_no_is_general = x.PBPComputer.article_no_is_general,
                    article_no_remarks = x.PBPComputer.article_no_remarks,
                    cs_machine_model = x.PBPComputer.cs_machine_model,
                    jig_design_id = y.jig_design_id,
                    jig_design_name = y.jig_design_name,
                    number_of_size_group = x.PBPComputer.number_of_size_group,
                    create_by = x.PBPComputer.create_by,
                    create_time = x.PBPComputer.create_time,
                    update_by = x.PBPComputer.update_by,
                    update_time = x.PBPComputer.update_time
                });


            if (!string.IsNullOrEmpty(search.Model))
            {
                var searchModel = search.Model.Trim().ToUpper();
                queryJoin = queryJoin.Where(x => x.model_no.Trim().ToUpper().Contains(searchModel) ||
                    x.model_name.Trim().ToUpper().Contains(searchModel));
            }
            if (!string.IsNullOrEmpty(search.DevSeason))
            {
                var searchModel = search.DevSeason.Trim().ToUpper();
                queryJoin = queryJoin.Where(x => x.dev_season.Trim().ToUpper().Contains(searchModel));
            }

            if (!string.IsNullOrEmpty(search.ProductionSeason))
            {
                var searchModel = search.ProductionSeason.Trim().ToUpper();
                queryJoin = queryJoin.Where(x => x.production_season.Trim().ToUpper().Contains(searchModel));
            }

            var result = await PagedList<ComputerStitchingSettingViewDTO>.CreateAsync(queryJoin, pagination.PageNumber, pagination.PageSize);
            return result;
        }

        public async Task<OperationResult> CreateComputerStitchingSetting(ComputerStitchingSettingViewDTO model)
        {
            if (await IsExists(model) == true)
                return new OperationResult { Success = false, Caption = "Computer Stitching Setting already exists." };
            var fn = new FunctionUtility();

            var filePath = model.factory_id.Trim() + "/ProductionBP/ComputerStitching/" + model.model_no.Trim();
            var fileName = Guid.NewGuid().ToString();

            var jigPhoto = await fn.UploadAsync(model.jig_photo_url, "uploaded/" + filePath, fileName);
            var csVideo = await fn.UploadAsync(model.cs_video_url, "uploaded/" + filePath, fileName);

            // No Image
            var fileNoImage = model.factory_id.Trim() + "/no-image.jpg";

            if (string.IsNullOrEmpty(model.jig_photo_url))
                model.jig_photo_url = fileNoImage;
            else
                model.jig_photo_url = filePath + "/" + jigPhoto;

            if (string.IsNullOrEmpty(model.cs_video_url))
                model.cs_video_url = fileNoImage;
            else
                model.cs_video_url = filePath + "/" + csVideo;

            var mapCreate = _mapper.Map<PBP_ComputerStitchingSetting>(model);
            _computerRepo.Add(mapCreate);
            try
            {
                await _computerRepo.SaveAll();
                return new OperationResult { Success = true, Caption = "Add this Computer Stitching Setting was Succeeded." };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OperationResult> UpdateComputerStitchingSetting(ComputerStitchingSettingViewDTO model)
        {
            if (await IsExists(model) == false)
                return new OperationResult { Success = false, Caption = "Computer Stitching Setting already exists." };
            var fn = new FunctionUtility();

            var filePath = model.factory_id.Trim() + "/ProductionBP/ComputerStitching/" + model.model_no.Trim();
            var fileName = Guid.NewGuid().ToString();

            var currentComputer = await _computerRepo.FindAll(x => x.factory_id.Trim() == model.factory_id.Trim() &&
                    x.model_no.Trim() == model.model_no.Trim() && x.stage_id.Trim() == model.stage_id.Trim() &&
                    x.operation_id.Trim() == model.operation_id.Trim() &&
                    x.cs_type_id.Trim() == model.cs_type_id.Trim() &&
                    x.cs_machine_type_id.Trim() == model.cs_machine_type_id.Trim())
                .Select(x => new { x.jig_photo_url, x.cs_video_url })
                .AsNoTracking().FirstOrDefaultAsync();

            if (!string.IsNullOrEmpty(model.jig_photo_url))
            {
                var jigPhoto = await fn.UploadAsync(model.jig_photo_url, "uploaded/" + filePath, fileName);
                model.jig_photo_url = filePath + "/" + jigPhoto;
            }
            else
                model.jig_photo_url = currentComputer.jig_photo_url;

            if (!string.IsNullOrEmpty(model.cs_video_url))
            {
                var csVideo = await fn.UploadAsync(model.cs_video_url, "uploaded/" + filePath, fileName);
                model.cs_video_url = filePath + "/" + csVideo;
            }
            else
                model.cs_video_url = currentComputer.cs_video_url;

            var mapUpdate = _mapper.Map<PBP_ComputerStitchingSetting>(model);
            _computerRepo.Update(mapUpdate);
            try
            {
                await _computerRepo.SaveAll();
                return new OperationResult { Success = true, Caption = "Computer Stitching Setting Was Successfully Update!" };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExists(ComputerStitchingSettingViewDTO model)
        {
            var result = await _computerRepo.FindAll(x => x.factory_id.Trim() == model.factory_id.Trim() &&
                    x.model_no.Trim() == model.model_no.Trim() && x.stage_id.Trim() == model.stage_id.Trim() &&
                    x.operation_id.Trim() == model.operation_id.Trim() &&
                    x.cs_type_id.Trim() == model.cs_type_id.Trim() &&
                    x.cs_machine_type_id.Trim() == model.cs_machine_type_id.Trim())
                .AsNoTracking().FirstOrDefaultAsync();
            return result == null ? false : true;
        }

        public async Task<List<KeyValuePair<string, string>>> GetModel()
        {
            var result = await _modelRepo.FindAll(x => x.is_active == true)
                .OrderBy(x => x.model_no)
                .Select(x => new KeyValuePair<string, string>(
                    x.model_no.Trim(), x.model_name.Trim()
                )).Distinct().ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetStage()
        {
            var result = await _stageRepo.FindAll(x => x.is_active == true)
                .OrderBy(x => x.sequence)
                .Select(x => new KeyValuePair<string, string>(
                    x.stage_id.Trim(), x.stage_name.Trim()
                )).Distinct().ToListAsync();
            return result;
        }

        public async Task<List<ModelOperationDTO>> GetCSOperation(string model_no, string stage_id)
        {
            var result = await _modelOperationRepo.FindAll(x => x.model_no.Trim() == model_no.Trim() &&
                    x.stage_id.Trim() == stage_id.Trim())
                .Select(x => new ModelOperationDTO
                {
                    operation_id = x.operation_id,
                    operation_name_en = x.operation_name_en,
                    operation_name_local = x.operation_name_local,
                    operation_name_zh = x.operation_name_zh,
                    critical_efficiency = x.critical_efficiency,
                    critical_quality = x.critical_quality
                }).Distinct().ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetCSType()
        {
            var result = await _cSTypeRepo.FindAll(x => x.is_active == true)
                .OrderBy(x => x.sequence)
                .Select(x => new KeyValuePair<string, string>(
                    x.cs_type_id.Trim(), x.cs_type_name.Trim()
                )).Distinct().ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetCSMachineType()
        {
            var result = await _cSMachineTypeRepo.FindAll(x => x.is_active == true)
                .OrderBy(x => x.sequence)
                .Select(x => new KeyValuePair<string, string>(
                    x.cs_machine_type_id.Trim(), x.cs_machine_type_name.Trim()
                )).Distinct().ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetMainUpperMaterialType()
        {
            var result = await _mainUpperMaterialRepo.FindAll(x => x.is_active == true)
                .OrderBy(x => x.sequence)
                .Select(x => new KeyValuePair<string, string>(
                    x.main_upper_material_type_id.Trim(), x.main_upper_material_type_name.Trim()
                )).Distinct().ToListAsync();
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetJigDesign()
        {
            var result = await _jigDesignTypeRepo.FindAll(x => x.is_active == true)
                .OrderBy(x => x.sequence)
                .Select(x => new KeyValuePair<string, string>(
                    x.jig_design_id.Trim(), x.jig_design_name.Trim()
                )).Distinct().ToListAsync();
            return result;
        }

        public async Task<OperationResult> UploadExcel(IFormFile file, string username, string factory)
        {
            // Lưu file vào wwwroot
            if (file == null)
                return new OperationResult(false, "File not found.", "Fail!");
            var extension = Path.GetExtension(file.FileName).ToLower();
            var uploadFile = $"Sample_ComputerStitchingSetting{extension}";
            string uploadPath = @"uploaded\excels";
            string folder = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath);

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

            int rows = ws.Cells.Rows.Count;
            var cell = ws.Cells;

            var listModel = await GetModel();
            var listStage = await GetStage();
            var listCsType = await GetCSType();
            var listCsMachineType = await GetCSMachineType();
            var listMainUpperMaterialType = await GetMainUpperMaterialType();
            var listJigDesign = await GetJigDesign();

            for (int i = 1; i < rows; i++)
            {
                var modelNo = cell[i, 0].StringValue.Trim();
                if (!(_modelRepo.FindAll(x => x.is_active == true && x.model_no.Trim() == modelNo).AsNoTracking().Any()))
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    return new OperationResult(false, "", "NotModel", modelNo);
                }

                var stageId = cell[i, 1].StringValue.Trim();

                var operationName = cell[i, 2].StringValue.Trim();
                var operationId = await _modelOperationRepo.FindAll(x =>
                        x.model_no.Trim() == modelNo.Trim() &&
                        x.stage_id.Trim() == stageId.Trim() &&
                        x.operation_name_en.Trim() == operationName.Trim())
                        .Select(x => x.operation_id.Trim()).FirstOrDefaultAsync();
                if (string.IsNullOrEmpty(operationId))
                {
                    if (File.Exists(filePath))
                        File.Delete(filePath);
                    return new OperationResult(false, "", "NotCS", operationName);
                }

                var csTypeId = cell[i, 3].StringValue.Trim();
                var csMachineTypeId = cell[i, 4].StringValue.Trim();
                var sopSetup = cell[i, 5].StringValue.Trim();
                var productionAdoption = cell[i, 6].StringValue.Trim();
                var isCriticalProcess = cell[i, 7].StringValue.Trim();
                var articleNoIsGeneral = cell[i, 8].StringValue.Trim();
                var articleNoRemarks = cell[i, 9].StringValue.Trim();
                var mainUpperMaterialTypeId = cell[i, 10].StringValue.Trim();
                var csMachineModel = cell[i, 11].StringValue.Trim();
                int csSpeedSetting, ofSizeGroup;
                var jigDesignId = cell[i, 14].StringValue.Trim();

                var isNumberSpeedSetting = int.TryParse(ws.Cells[i, 12].StringValue?.Trim(), out int numberSpeedSetting);
                var isNumberSizeGroup = int.TryParse(ws.Cells[i, 13].StringValue?.Trim(), out int numberSizeGroup);
                if (isNumberSpeedSetting && isNumberSizeGroup)
                {
                    csSpeedSetting = numberSpeedSetting;
                    ofSizeGroup = numberSizeGroup;
                }
                else
                    continue;

                // Check Null or Empty
                string[] checkArray = new string[] { modelNo, stageId, operationName, csTypeId, csMachineTypeId, sopSetup, productionAdoption, isCriticalProcess, articleNoIsGeneral, mainUpperMaterialTypeId, csMachineModel, jigDesignId };
                if (checkArray.Any(x => string.IsNullOrEmpty(x)))
                    continue;

                if (!listModel.Any(x => x.Key.Trim() == modelNo) &&
                    !listStage.Any(x => x.Key.Trim() == stageId) &&
                    !listCsType.Any(x => x.Key.Trim() == csTypeId) &&
                    !listCsMachineType.Any(x => x.Key.Trim() == csMachineTypeId) &&
                    !listMainUpperMaterialType.Any(x => x.Key.Trim() == mainUpperMaterialTypeId) &&
                    !listJigDesign.Any(x => x.Key.Trim() == jigDesignId))
                    continue;

                // Check Exist
                var checkExist = await _computerRepo.FindAll(x => x.model_no.Trim() == modelNo && x.stage_id.Trim() == stageId && x.operation_id.Trim() == operationId && x.cs_type_id.Trim() == csTypeId && x.cs_machine_type_id.Trim() == csMachineTypeId).FirstOrDefaultAsync();

                if (checkExist != null)
                    continue;
                else
                {
                    if (articleNoIsGeneral.Trim() == "Yes")
                        articleNoRemarks = "";
                    else
                    {
                        if (string.IsNullOrEmpty(articleNoRemarks))
                            continue;
                    }
                    var computer = new PBP_ComputerStitchingSetting
                    {
                        factory_id = factory,
                        model_no = modelNo.Trim(),
                        stage_id = stageId.Trim(),
                        operation_id = operationId.Trim(),
                        cs_type_id = csTypeId.Trim(),
                        cs_machine_type_id = csMachineTypeId.Trim(),
                        sop_setup = sopSetup.Trim() == "Yes" ? true : false,
                        production_adoption = productionAdoption.Trim() == "Yes" ? true : false,
                        is_critical_process = isCriticalProcess.Trim() == "Yes" ? true : false,
                        article_no_is_general = articleNoIsGeneral.Trim() == "Yes" ? true : false,
                        article_no_remarks = articleNoRemarks.Trim(),
                        main_upper_material_type_id = mainUpperMaterialTypeId.Trim(),
                        cs_machine_model = csMachineModel.Trim(),
                        cs_speed_setting_rpm = csSpeedSetting,
                        number_of_size_group = ofSizeGroup,
                        jig_design_id = jigDesignId.Trim(),
                        jig_photo_url = factory + "/no-image.jpg",
                        cs_video_url = factory + "/no-image.jpg",
                        create_by = username,
                        create_time = DateTime.Now,
                        update_by = username,
                        update_time = DateTime.Now
                    };
                    _computerRepo.Add(computer);
                }
            }
            if (await _computerRepo.SaveAll())
                return new OperationResult { Success = true, Message = "Upload Successfully", Caption = "Success" };
            return new OperationResult { Success = false, Message = "Upload failed on save. Please check the excel data again.", Caption = "Error" };
        }
    }
}