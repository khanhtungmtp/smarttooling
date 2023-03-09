using Aspose.Cells;
using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.ProductionBP;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class ModelOperationService : IModelOperationService
    {
        private readonly IModelOperationRepository _repoModelOperation;
        private readonly IProcessTypeRepository _repoProcessType;
        private readonly IModelRepository _repoModel;
        private readonly IStageRepository _repoStage;
        private readonly IKaizenRepository _repoKaizen;
        private readonly IMeasurement_RFTRepository _repoMeasurement;
        private readonly IConfiguration _configuration;
        private readonly IFunctionUtility _functionUtility;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPBPComputerStitchingSettingRepository _repoPBPComputerStitchingSetting;
        private readonly IBLCriticalProcessAnalysisRepository _repoBLCriticalProcessAnalysis;
        private readonly IBLRolloutAuditRepository _repoBLRolloutAudit;
        private readonly IBLRolloutProgressRepository _repoBLRolloutProgress;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private string factory;
        public ModelOperationService(
            IModelOperationRepository repoModelOperation,
            IMapper mapper,
            MapperConfiguration configMapper,
            IModelRepository repoModel,
            IStageRepository repoStage,
            IProcessTypeRepository repoProcessType,
            IKaizenRepository repoKaizen,
            IMeasurement_RFTRepository repoMeasurement,
            IPBPComputerStitchingSettingRepository repoPBPComputerStitchingSetting,
            IBLCriticalProcessAnalysisRepository repoBLCriticalProcessAnalysis,
            IBLRolloutAuditRepository repoBLRolloutAudit,
            IBLRolloutProgressRepository repoBLRolloutProgress,
            IConfiguration configuration,
            IFunctionUtility functionUtility,
            IWebHostEnvironment webHostEnvironment)
        {
            _repoModelOperation = repoModelOperation;
            _mapper = mapper;
            _repoModel = repoModel;
            _repoStage = repoStage;
            _repoKaizen = repoKaizen;
            _repoMeasurement = repoMeasurement;
            _configuration = configuration;
            _functionUtility = functionUtility;
            _webHostEnvironment = webHostEnvironment;
            _repoPBPComputerStitchingSetting = repoPBPComputerStitchingSetting;
            _repoBLCriticalProcessAnalysis = repoBLCriticalProcessAnalysis;
            _repoBLRolloutAudit = repoBLRolloutAudit;
            _repoBLRolloutProgress = repoBLRolloutProgress;
            _configMapper = configMapper;
            _repoProcessType = repoProcessType;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }

        public async Task<PageListUtility<ModelOperationDTO>> SearchModelOperation(PaginationParams param, ModelOperationParam modelParam)
        {
            var pred_Model = PredicateBuilder.New<Model_Operation>(true);

            if (!String.IsNullOrEmpty(modelParam.model_search))
                pred_Model.And(x => x.model_no == modelParam.model_search);

            if (!String.IsNullOrEmpty(modelParam.stage))
                pred_Model.And(x => x.stage_id == modelParam.stage);

            var listOperation = _repoModelOperation.FindAll(pred_Model).OrderBy(x => x.sequence).ThenBy(x => x.operation_id);
            var listProcessType = _repoProcessType.FindAll(x => x.factory_id.Trim() == factory && x.is_active == true);
            var listData = listOperation.Join(listProcessType, x => x.process_type_id, y => y.process_type_id,
                (x, y) => new ModelOperationDTO
                {
                    factory_id = x.factory_id,
                    model_no = x.model_no,
                    stage_id = x.stage_id,
                    operation_id = x.operation_id,
                    process_type_id = y.process_type_id,
                    process_type_name = y.process_type_name_en,
                    operation_name_local = x.operation_name_local,
                    operation_name_en = x.operation_name_en,
                    operation_name_zh = x.operation_name_zh,
                    sop_no = x.sop_no,
                    critical_quality = x.critical_quality,
                    critical_efficiency = x.critical_efficiency,
                    sequence = x.sequence,
                    create_by = x.create_by,
                    create_time = x.create_time,
                    update_time = x.update_time,
                    update_by = x.update_by,
                });

            return await PageListUtility<ModelOperationDTO>.PageListAsync(listData, param.PageNumber, param.PageSize);
        }

        public async Task<bool> Add(ModelOperationDTO model)
        {
            var modelParam = new ModelOperationEditParam();
            modelParam.factory_id = model.factory_id;
            modelParam.model_no = model.model_no;
            modelParam.stage_id = model.stage_id;
            modelParam.operation_id = model.operation_id;
            var operation = await GetModelOperation(modelParam);
            if (operation == null)
            {
                var modelOperation = _mapper.Map<Model_Operation>(model);
                _repoModelOperation.Add(modelOperation);
                return await _repoModelOperation.SaveAll();
            }
            else return false;
        }

        public async Task<bool> Update(ModelOperationDTO model)
        {
            var modelOperation = _mapper.Map<Model_Operation>(model);
            _repoModelOperation.Update(modelOperation);
            return await _repoModelOperation.SaveAll();
        }
        // public async Task<bool> Delete(ModelOperationDTO operation)
        // {
        //     if (await CheckModelOperationIsUsing(operation))
        //     {
        //         return false;
        //     }
        //     else
        //     {
        //         var modelOperation = _mapper.Map<Model_Operation>(operation);
        //         _repoModelOperation.Remove(modelOperation);
        //         return await _repoModelOperation.SaveAll();
        //     }
        // }
        public async Task<object> GetAllProcessType()
        {
            return await _repoProcessType.FindAll(x => x.factory_id.Trim() == factory && x.is_active == true)
            .Select(x => new { x.process_type_id, x.process_type_name_en, x.sequence }).Distinct().OrderBy(x => x.sequence).ToListAsync();
        }

        public async Task<ModelOperationDTO> GetModelOperation(ModelOperationEditParam modelOperationEditParam)
        {
            var data = await _repoModelOperation.GetByModelOperation(modelOperationEditParam);
            var models = _mapper.Map<Model_Operation, ModelOperationDTO>(data);
            return models;
        }

        // public async Task<bool> CheckModelOperationIsUsing(ModelOperationDTO operation)
        // {
        //     if (await _repoKaizen.CheckExistsKaizen(operation) ||
        //          await _repoMeasurement.CheckExistsRTF(operation) ||
        //          await _repoPBPComputerStitchingSetting.CheckExistsPBPComputerStitchingSetting(operation) ||
        //          await _repoBLCriticalProcessAnalysis.CheckExistsBLCriticalProcessAnalysis(operation) ||
        //          await _repoBLRolloutAudit.CheckExistsBLRolloutAudit(operation) ||
        //          await _repoBLRolloutProgress.CheckExistsBLRolloutProgress(operation)
        //        )
        //         return true;
        //     return false;
        // }

        public async Task<List<ModelOperationDTO>> GetExportExcelData(ModelOperationParam param)
        {
            var pred = PredicateBuilder.New<Model_Operation>(true);

            if (!string.IsNullOrEmpty(param?.model_search?.Trim()))
                pred = pred.And(x => x.model_no.Trim() == param.model_search.Trim());

            if (!string.IsNullOrEmpty(param?.stage?.Trim()))
                pred = pred.And(x => x.stage_id.Trim() == param.stage.Trim());

            var result = await _repoModelOperation.FindAll(pred)
                .Join(_repoModel.FindAll(),
                    x => new { factory_id = x.factory_id.Trim(), model_no = x.model_no.Trim() },
                    y => new { factory_id = y.factory_id.Trim(), model_no = y.model_no.Trim() },
                    (x, y) => new { ModelOperation = x, Model = y })
                .Join(_repoStage.FindAll(),
                    x => new { factory_id = x.ModelOperation.factory_id.Trim(), stage_id = x.ModelOperation.stage_id.Trim() },
                    y => new { factory_id = y.factory_id.Trim(), stage_id = y.stage_id.Trim() },
                    (x, y) => new { x.ModelOperation, x.Model, Stage = y })
                .Join(_repoProcessType.FindAll(),
                    x => new { factory_id = x.ModelOperation.factory_id.Trim(), process_type_id = x.ModelOperation.process_type_id.Trim() },
                    y => new { factory_id = y.factory_id.Trim(), process_type_id = y.process_type_id.Trim() },
                    (x, y) => new { x.ModelOperation, x.Model, x.Stage, ProcessType = y })
                .Select(x => new ModelOperationDTO
                {
                    factory_id = x.ModelOperation.factory_id.Trim(),
                    model_no = x.ModelOperation.model_no.Trim(),
                    model_name = x.Model.model_name.Trim(),
                    stage_name = x.Stage.stage_name.Trim(),
                    operation_id = x.ModelOperation.operation_id.Trim(),
                    process_type_name_en = x.ProcessType.process_type_name_en.Trim(),
                    operation_name_local = x.ModelOperation.operation_name_local.Trim(),
                    operation_name_en = x.ModelOperation.operation_name_en.Trim(),
                    operation_name_zh = x.ModelOperation.operation_name_zh.Trim(),
                    sop_no = x.ModelOperation.sop_no.Trim(),
                    critical_quality_bit = x.ModelOperation.critical_quality ? 1 : 0,
                    critical_efficiency_bit = x.ModelOperation.critical_efficiency ? 1 : 0,
                    sequence = x.ModelOperation.sequence
                }).ToListAsync();

            return result;
        }

        public async Task<OperationResult> UploadExcel(IFormFile file, string user)
        {
            if (file == null)
                return new OperationResult(false, "FILE_NOT_FOUND");

            var uploadFile = $"Sample_ModelOperation";
            var uploadPath = @"uploaded\excels";
            var fileName = await _functionUtility.UploadAsync(file, uploadPath, uploadFile);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, uploadPath, fileName);

            var designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(filePath);
            var ws = designer.Workbook.Worksheets[0];
            int rows = ws.Cells.MaxDataRow;
            var dtNow = DateTime.Now;

            if (rows < 1)
                return new OperationResult(false, "FILE_IS_NULL_OR_EMPTY");

            for (var i = 1; i <= rows; i++)
            {
                var excelOperation = new Model_Operation
                {
                    factory_id = ws.Cells[i, 0].StringValue?.Trim(),
                    model_no = ws.Cells[i, 1].StringValue?.Trim(),
                    stage_id = ws.Cells[i, 2].StringValue?.Trim(),
                    operation_id = ws.Cells[i, 3].StringValue?.Trim(),
                    process_type_id = ws.Cells[i, 4].StringValue?.Trim(),
                    operation_name_local = ws.Cells[i, 5].StringValue?.Trim(),
                    operation_name_en = ws.Cells[i, 6].StringValue?.Trim(),
                    operation_name_zh = ws.Cells[i, 7].StringValue?.Trim(),
                    sop_no = ws.Cells[i, 8].StringValue?.Trim(),
                    critical_quality = ws.Cells[i, 9].StringValue?.Trim() == "YES",
                    critical_efficiency = ws.Cells[i, 10].StringValue?.Trim() == "YES",
                    sequence = ws.Cells[i, 11].IntValue,
                    create_by = user,
                    create_time = dtNow,
                    update_by = user,
                    update_time = dtNow,
                };

                var modelOperation = _repoModelOperation.FindSingle(x =>
                    x.factory_id.Trim() == excelOperation.factory_id &&
                    x.model_no.Trim() == excelOperation.model_no &&
                    x.stage_id.Trim() == excelOperation.stage_id &&
                    x.operation_id.Trim() == excelOperation.operation_id);

                if (modelOperation != null)
                {
                    modelOperation.process_type_id = excelOperation.process_type_id;
                    modelOperation.operation_name_local = excelOperation.operation_name_local;
                    modelOperation.operation_name_en = excelOperation.operation_name_en;
                    modelOperation.operation_name_zh = excelOperation.operation_name_zh;
                    modelOperation.sop_no = excelOperation.sop_no;
                    modelOperation.critical_quality = excelOperation.critical_quality;
                    modelOperation.critical_efficiency = excelOperation.critical_efficiency;
                    modelOperation.sequence = excelOperation.sequence;
                    modelOperation.update_by = user;
                    modelOperation.update_time = dtNow;

                    _repoModelOperation.Update(modelOperation);
                }
                else
                {
                    _repoModelOperation.Add(excelOperation);
                }
            }

            await _repoModelOperation.SaveAll();
            return new OperationResult(true);
        }
    }
}