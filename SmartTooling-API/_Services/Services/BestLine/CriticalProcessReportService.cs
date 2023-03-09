using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Helpers.Utilities;
using SmartTooling_API.Models.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class CriticalProcessReportService : ICriticalProcessReportService
    {
        private readonly IBLLinesRepository _blLinesRepository;
        private readonly IBLLineTypeRepository _blLineTypeRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IModelOperationRepository _modelOperationRepository;
        private readonly IBLCriticalProcessAnalysisRepository _blCriticalProcessAnalysisRepository;
        private readonly IKaizenRepository _kaizenRepository;
        private readonly IProcessTypeRepository _processTypeRepository;
        private readonly IFactoryRepository _factoryRepository;
        private readonly IConfiguration _configuration;
        private readonly IImageUrlUtility _imageUrlUtility;
        private readonly IMapper _mapper;

        public CriticalProcessReportService(
            IBLLinesRepository iBLLinesRepository,
            IBLLineTypeRepository iBLLineTypeRepository,
            IModelRepository modelRepository,
            IStageRepository stageRepository,
            IModelOperationRepository modelOperationRepository,
            IBLCriticalProcessAnalysisRepository bLCriticalProcessAnalysisRepository,
            IKaizenRepository kaizenRepository,
            IProcessTypeRepository processTypeRepository,
            IFactoryRepository factoryRepository,
            IConfiguration configuration,
            IImageUrlUtility imageUrlUtility,
            IMapper mapper)
        {
            _blLinesRepository = iBLLinesRepository;
            _blLineTypeRepository = iBLLineTypeRepository;
            _modelRepository = modelRepository;
            _stageRepository = stageRepository;
            _modelOperationRepository = modelOperationRepository;
            _blCriticalProcessAnalysisRepository = bLCriticalProcessAnalysisRepository;
            _kaizenRepository = kaizenRepository;
            _processTypeRepository = processTypeRepository;
            _factoryRepository = factoryRepository;
            _configuration = configuration;
            _imageUrlUtility = imageUrlUtility;
            _mapper = mapper;
        }



        // public async Task<List<KeyValuePair<string, string>>> GetAllFactory()
        // {
        //     var data = await _factoryRepository.FindAll().Select(x => new KeyValuePair<string, string>(x.factory_id.Trim(), x.factory_name.Trim())).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<CriticalProcessReportDetailDTO> GetDetail(CriticalProcessReportParam searchParam)
        // {
        //     var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(searchParam.factory_id.Trim());
        //     var pred = PredicateBuilder.New<BL_Critical_Process_Analysis>(true);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = searchParam.factory_id.Trim();
        //     if (!string.IsNullOrEmpty(searchParam.line_no))
        //     {
        //         pred = pred.And(x => x.line_id.Trim() == searchParam.line_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_type))
        //     {
        //         pred = pred.And(x => x.line_type_id.Trim() == searchParam.line_type.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.model_no))
        //     {
        //         pred = pred.And(x => x.model_no.ToUpper().Trim() == searchParam.model_no.ToUpper().Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.stage))
        //     {
        //         pred = pred.And(x => x.stage_id.Trim() == searchParam.stage.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.operation))
        //     {
        //         pred = pred.And(x => x.operation_id.Trim() == searchParam.operation.Trim());
        //     }
        //     var Critical = _blCriticalProcessAnalysisRepository.FindAll(pred);
        //     var Line = _blLinesRepository.FindAll();
        //     var LineType = _blLineTypeRepository.FindAll();
        //     var Model = _modelRepository.FindAll();
        //     var Stage = _stageRepository.FindAll();
        //     var ModelOperation = _modelOperationRepository.FindAll();

        //     var data = Critical.GroupJoin(
        //         Line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { Critical = x, Line = y })
        //         .SelectMany(
        //             x => x.Line.DefaultIfEmpty(),
        //             (x, y) => new { x.Critical, Line = y })
        //             .GroupJoin(
        //                 LineType,
        //                 x => new { x.Critical.factory_id, x.Critical.line_type_id },
        //                 y => new { y.factory_id, y.line_type_id },
        //                 (x, y) => new { x.Critical, x.Line, LineType = y })
        //                 .SelectMany(
        //                     x => x.LineType.DefaultIfEmpty(),
        //                     (x, y) => new { x.Critical, x.Line, LineType = y })
        //                     .GroupJoin(
        //                         Model,
        //                         x => new { x.Critical.factory_id, x.Critical.model_no },
        //                         y => new { y.factory_id, y.model_no },
        //                         (x, y) => new { x.Critical, x.Line, x.LineType, Model = y })
        //                         .SelectMany(
        //                             x => x.Model.DefaultIfEmpty(),
        //                             (x, y) => new { x.Critical, x.Line, x.LineType, Model = y })
        //                             .GroupJoin(
        //                                 Stage,
        //                                 x => new { x.Critical.factory_id, x.Critical.stage_id },
        //                                 y => new { y.factory_id, y.stage_id },
        //                                 (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, Stage = y })
        //                                 .SelectMany(
        //                                     x => x.Stage.DefaultIfEmpty(),
        //                                     (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, Stage = y })
        //                                     .GroupJoin(
        //                                         ModelOperation,
        //                                         x => new { x.Critical.factory_id, x.Critical.model_no, x.Critical.stage_id, x.Critical.operation_id },
        //                                         y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
        //                                         (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, x.Stage, ModelOperation = y })
        //                                         .SelectMany(
        //                                             x => x.ModelOperation.DefaultIfEmpty(),
        //                                             (x, y) => new CriticalProcessReportDetailDTO
        //                                             {
        //                                                 factory = x.Critical.factory_id,
        //                                                 line_id = x.Line.line_id,
        //                                                 line_name = x.Line.line_name,
        //                                                 line_type_id = x.LineType.line_type_id,
        //                                                 line_type_name = x.LineType.line_type_name,
        //                                                 model_no = x.Critical.model_no,
        //                                                 model_name = x.Model.model_name,
        //                                                 stage_id = x.Stage.stage_id,
        //                                                 stage_name = x.Stage.stage_name,
        //                                                 operation_id = x.Critical.operation_id,
        //                                                 operation_name_en = y.operation_name_en,
        //                                                 operation_name_zh = y.operation_name_zh,
        //                                                 operation_name_local = y.operation_name_local,
        //                                                 machine_media_url = x.Critical.machine_media_url,
        //                                                 machine_remarks = x.Critical.machine_remarks,
        //                                                 man_media_url = x.Critical.man_media_url,
        //                                                 man_remarks = x.Critical.man_remarks,
        //                                                 method_media_url = x.Critical.method_media_url,
        //                                                 method_remarks = x.Critical.method_remarks,
        //                                                 material_media_url = x.Critical.material_media_url,
        //                                                 material_remarks = x.Critical.material_remarks,
        //                                                 man_media_url_result = imageUrl + x.Critical.man_media_url,
        //                                                 method_media_url_result = imageUrl + x.Critical.method_media_url,
        //                                                 machine_media_url_result = imageUrl + x.Critical.machine_media_url,
        //                                                 material_media_url_result = imageUrl + x.Critical.material_media_url,
        //                                             }
        //                                         );
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return await data.FirstOrDefaultAsync();
        // }

        // public async Task<PagedList<CriticalProcessReportKaizenDTO>> GetKaizen(PaginationParams pagination, CriticalProcessReportParam searchParam)
        // {
        //     var pred = PredicateBuilder.New<BL_Critical_Process_Analysis>(true);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = searchParam.factory_id.Trim();
        //     if (!string.IsNullOrEmpty(searchParam.line_no))
        //     {
        //         pred = pred.And(x => x.line_id.Trim() == searchParam.line_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_type))
        //     {
        //         pred = pred.And(x => x.line_type_id.Trim() == searchParam.line_type.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.model_no))
        //     {
        //         pred = pred.And(x => x.model_no.ToUpper().Trim() == searchParam.model_no.ToUpper().Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.stage))
        //     {
        //         pred = pred.And(x => x.stage_id.Trim() == searchParam.stage.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.operation))
        //     {
        //         pred = pred.And(x => x.operation_id.Trim() == searchParam.operation.Trim());
        //     }
        //     var Critical = _blCriticalProcessAnalysisRepository.FindAll(pred);
        //     var Line = _blLinesRepository.FindAll();
        //     var LineType = _blLineTypeRepository.FindAll();
        //     var Model = _modelRepository.FindAll();
        //     var Stage = _stageRepository.FindAll();
        //     var ModelOperation = _modelOperationRepository.FindAll();
        //     var Kaizen = _kaizenRepository.FindAll();
        //     var ProcessType = _processTypeRepository.FindAll();
        //     var data = Critical.GroupJoin(
        //         Line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { Critical = x, Line = y })
        //         .SelectMany(
        //             x => x.Line.DefaultIfEmpty(),
        //             (x, y) => new { x.Critical, Line = y })
        //             .GroupJoin(
        //                 LineType,
        //                 x => new { x.Critical.factory_id, x.Critical.line_type_id },
        //                 y => new { y.factory_id, y.line_type_id },
        //                 (x, y) => new { x.Critical, x.Line, LineType = y })
        //                 .SelectMany(
        //                     x => x.LineType.DefaultIfEmpty(),
        //                     (x, y) => new { x.Critical, x.Line, LineType = y })
        //                     .GroupJoin(
        //                         Model,
        //                         x => new { x.Critical.factory_id, x.Critical.model_no },
        //                         y => new { y.factory_id, y.model_no },
        //                         (x, y) => new { x.Critical, x.Line, x.LineType, Model = y })
        //                         .SelectMany(
        //                             x => x.Model.DefaultIfEmpty(),
        //                             (x, y) => new { x.Critical, x.Line, x.LineType, Model = y })
        //                             .GroupJoin(
        //                                 Stage,
        //                                 x => new { x.Critical.factory_id, x.Critical.stage_id },
        //                                 y => new { y.factory_id, y.stage_id },
        //                                 (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, Stage = y })
        //                                 .SelectMany(
        //                                     x => x.Stage.DefaultIfEmpty(),
        //                                     (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, Stage = y })
        //                                     .GroupJoin(
        //                                         ModelOperation,
        //                                         x => new { x.Critical.factory_id, x.Critical.model_no, x.Critical.stage_id, x.Critical.operation_id },
        //                                         y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
        //                                         (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, x.Stage, ModelOperation = y })
        //                                         .SelectMany(
        //                                             x => x.ModelOperation.DefaultIfEmpty(),
        //                                             (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, x.Stage, ModelOperation = y })
        //                                             .GroupJoin(
        //                                                 Kaizen,
        //                                                 x => new { x.Critical.factory_id, x.Critical.model_no, x.Critical.stage_id, x.Critical.operation_id },
        //                                                 y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
        //                                                 (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, x.Stage, x.ModelOperation, Kaizen = y })
        //                                                 .SelectMany(
        //                                                     x => x.Kaizen.DefaultIfEmpty(),
        //                                                     (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, x.Stage, x.ModelOperation, Kaizen = y })
        //                                                     .GroupJoin(
        //                                                         ProcessType,
        //                                                         x => new { x.Critical.factory_id, x.ModelOperation.process_type_id },
        //                                                         y => new { y.factory_id, y.process_type_id },
        //                                                         (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, x.Stage, x.ModelOperation, x.Kaizen, ProcessType = y })
        //                                                         .SelectMany(
        //                                                             x => x.ProcessType.DefaultIfEmpty(),
        //                                                             (x, y) => new CriticalProcessReportKaizenDTO
        //                                                             {
        //                                                                 factory_id = x.Critical.factory_id,
        //                                                                 line_id = x.Line.line_id,
        //                                                                 line_name = x.Line.line_name,
        //                                                                 line_type_id = x.LineType.line_type_id,
        //                                                                 line_type_name = x.LineType.line_type_name,
        //                                                                 model_no = x.Critical.model_no,
        //                                                                 model_name = x.Model.model_name,
        //                                                                 stage_id = x.Stage.stage_id,
        //                                                                 stage_name = x.Stage.stage_name,
        //                                                                 operation_id = x.ModelOperation.operation_id,
        //                                                                 operation_name_local = x.ModelOperation.operation_name_local,
        //                                                                 operation_name_en = x.ModelOperation.operation_name_en,
        //                                                                 operation_name_zh = x.ModelOperation.operation_name_zh,
        //                                                                 serial_no = x.Kaizen.serial_no,
        //                                                                 kaizen_description = x.Kaizen.kaizen_description,
        //                                                                 start_date = x.Kaizen.start_date,
        //                                                                 process_type_name_en = y.process_type_name_en,
        //                                                                 process_tct_sec = x.Kaizen.process_tct_sec,
        //                                                                 ct_before_sec = x.Kaizen.ct_before_sec,
        //                                                                 ct_after_sec = x.Kaizen.ct_after_sec,
        //                                                                 rft_before_percent = x.Kaizen.rft_before_percent,
        //                                                                 rft_after_percent = x.Kaizen.rft_after_percent,
        //                                                                 line_roll_out_percent = x.Kaizen.line_roll_out_percent,
        //                                                                 clicks_times = x.Kaizen.clicks_times,
        //                                                             }
        //                                                         );
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return await PagedList<CriticalProcessReportKaizenDTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetLineNo(string factory_id)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
        //     var data = await _blLinesRepository.FindAll().OrderBy(x => x.sequence)
        //                                                 .Select(x => new KeyValuePair<string, string>(x.line_id.Trim(), x.line_name.Trim())).Distinct().ToListAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return data;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetLineType(string factory_id)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
        //     var data = await _blLineTypeRepository.FindAll().OrderBy(x => x.sequence)
        //                                                     .Select(x => new KeyValuePair<string, string>(x.line_type_id.Trim(), x.line_type_name.Trim())).Distinct().ToListAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return data;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetStage(string factory_id)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
        //     var data = await _stageRepository.FindAll(x => x.is_active == true).OrderBy(x => x.sequence)
        //                                                     .Select(x => new KeyValuePair<string, string>(x.stage_id.Trim(), x.stage_name.Trim())).Distinct().ToListAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return data;
        // }

        // public async Task<PagedList<CriticalProcessReportDTO>> Search(PaginationParams pagination, CriticalProcessReportParam searchParam)
        // {
        //     var pred = PredicateBuilder.New<BL_Critical_Process_Analysis>(true);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = searchParam.factory_id.Trim();
        //     if (!string.IsNullOrEmpty(searchParam.factory_id))
        //     {
        //         pred = pred.And(x => x.factory_id.Trim() == searchParam.factory_id.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_no))
        //     {
        //         pred = pred.And(x => x.line_id.Trim() == searchParam.line_no.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.line_type))
        //     {
        //         pred = pred.And(x => x.line_type_id.Trim() == searchParam.line_type.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(searchParam.stage))
        //     {
        //         pred = pred.And(x => x.stage_id.Trim() == searchParam.stage.Trim());
        //     }
        //     var Critical = _blCriticalProcessAnalysisRepository.FindAll(pred);
        //     var Line = _blLinesRepository.FindAll();
        //     var LineType = _blLineTypeRepository.FindAll();
        //     var Model = _modelRepository.FindAll();
        //     var Stage = _stageRepository.FindAll();
        //     var ModelOperation = _modelOperationRepository.FindAll();

        //     var data = Critical.GroupJoin(
        //         Line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { Critical = x, Line = y })
        //         .SelectMany(
        //             x => x.Line.DefaultIfEmpty(),
        //             (x, y) => new { x.Critical, Line = y })
        //             .GroupJoin(
        //                 LineType,
        //                 x => new { x.Critical.factory_id, x.Critical.line_type_id },
        //                 y => new { y.factory_id, y.line_type_id },
        //                 (x, y) => new { x.Critical, x.Line, LineType = y })
        //                 .SelectMany(
        //                     x => x.LineType.DefaultIfEmpty(),
        //                     (x, y) => new { x.Critical, x.Line, LineType = y })
        //                     .GroupJoin(
        //                         Model,
        //                         x => new { x.Critical.factory_id, x.Critical.model_no },
        //                         y => new { y.factory_id, y.model_no },
        //                         (x, y) => new { x.Critical, x.Line, x.LineType, Model = y })
        //                         .SelectMany(
        //                             x => x.Model.DefaultIfEmpty(),
        //                             (x, y) => new { x.Critical, x.Line, x.LineType, Model = y })
        //                             .GroupJoin(
        //                                 Stage,
        //                                 x => new { x.Critical.factory_id, x.Critical.stage_id },
        //                                 y => new { y.factory_id, y.stage_id },
        //                                 (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, Stage = y })
        //                                 .SelectMany(
        //                                     x => x.Stage.DefaultIfEmpty(),
        //                                     (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, Stage = y })
        //                                     .GroupJoin(
        //                                         ModelOperation,
        //                                         x => new { x.Critical.factory_id, x.Critical.model_no, x.Critical.stage_id, x.Critical.operation_id },
        //                                         y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
        //                                         (x, y) => new { x.Critical, x.Line, x.LineType, x.Model, x.Stage, ModelOperation = y })
        //                                         .SelectMany(
        //                                             x => x.ModelOperation.DefaultIfEmpty(),
        //                                             (x, y) => new CriticalProcessReportDTO
        //                                             {
        //                                                 factory_id = x.Critical.factory_id,
        //                                                 line_id = x.Line.line_id,
        //                                                 line_name = x.Line.line_name,
        //                                                 line_type_id = x.LineType.line_type_id,
        //                                                 line_type_name = x.LineType.line_type_name,
        //                                                 model_no = x.Critical.model_no,
        //                                                 model_name = x.Model.model_name,
        //                                                 stage_id = x.Stage.stage_id,
        //                                                 stage_name = x.Stage.stage_name,
        //                                                 operation_id = y.operation_id,
        //                                                 operation_name_en = y.operation_name_en,
        //                                                 operation_name_local = y.operation_name_local,
        //                                                 operation_name_zh = y.operation_name_zh,
        //                                             }
        //                                         );
        //     if (!string.IsNullOrEmpty(searchParam.model_no))
        //     {
        //         data = data.Where(x => x.model_no.ToUpper().Trim().Contains(searchParam.model_no.ToUpper().Trim()) || x.model_name.Contains(searchParam.model_no.ToUpper().Trim()));
        //     }
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return await PagedList<CriticalProcessReportDTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize);
        // }

        // public async Task<CriticalProcessReportKaizenDetailDTO> GetKaizenDetail(string factory_id, string model_no, string serial_no)
        // {
        //     var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(factory_id.Trim());
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
        //     var modelOperations = _modelOperationRepository.FindAll();
        //     var kaizens = _kaizenRepository.FindAll();
        //     var models = _modelRepository.FindAll();

        //     var data = await (from a in kaizens
        //                       join b in models
        //                       on new { factoryId = a.factory_id.Trim(), modelNo = a.model_no.Trim() }
        //                       equals new { factoryId = b.factory_id.Trim(), modelNo = b.model_no.Trim() }
        //                       join c in modelOperations
        //                       on new { factoryId = a.factory_id.Trim(), modelNo = a.model_no.Trim(), operationId = a.operation_id.Trim(), stageId = a.stage_id.Trim() }
        //                       equals new { factoryId = c.factory_id.Trim(), modelNo = c.model_no.Trim(), operationId = c.operation_id.Trim(), stageId = c.stage_id.Trim() }
        //                       where a.factory_id.Trim() == factory_id.Trim() && a.model_no.Trim() == model_no.Trim()
        //                       && a.serial_no.ToString() == serial_no.Trim()
        //                       select new
        //                       {
        //                           kaizen = a,
        //                           model = b,
        //                           modelOperation = c
        //                       }).FirstOrDefaultAsync();

        //     var result = new CriticalProcessReportKaizenDetailDTO
        //     {
        //         Kaizen = _mapper.Map<KaizenDTO>(data.kaizen),
        //         Model = _mapper.Map<ModelDTO>(data.model),
        //         ModelOperation = _mapper.Map<ModelOperationDTO>(data.modelOperation)
        //     };

        //     result.Kaizen.after_media_result = imageUrl + result.Kaizen.after_media;
        //     result.Kaizen.before_media_result = imageUrl + result.Kaizen.before_media;
        //     result.Model.model_picture_result = imageUrl + result.Model.model_picture;

        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return result;
        // }

    }
}