using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Utilities;
using SmartTooling_API.Models.BestLine;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class BLRolloutReportService : IBLRolloutReportService
    {
        private readonly IBLRolloutProgressRepository _rolloutReportRepo;
        private readonly IFactoryRepository _factoryRepo;
        private readonly IBLLinesRepository _linesRepo;
        private readonly IBLLineTypeRepository _lineTypeRepo;
        private readonly IModelRepository _modelRepo;
        private readonly IModelOperationRepository _modelOperationRepo;
        private readonly IStageRepository _stageRepo;
        private readonly IBLCriticalProcessAnalysisRepository _criticalRepo;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IImageUrlUtility _imageUrlUtility;

        // public BLRolloutReportService(IBLRolloutProgressRepository rolloutReportRepo, IFactoryRepository factoryRepo, IBLLinesRepository linesRepo, IBLLineTypeRepository lineTypeRepo, IModelRepository modelRepo, IModelOperationRepository modelOperationRepo, IStageRepository stageRepo, IBLCriticalProcessAnalysisRepository criticalRepo, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IMapper mapper, IImageUrlUtility imageUrlUtility)
        // {
        //     _rolloutReportRepo = rolloutReportRepo;
        //     _factoryRepo = factoryRepo;
        //     _linesRepo = linesRepo;
        //     _lineTypeRepo = lineTypeRepo;
        //     _modelRepo = modelRepo;
        //     _modelOperationRepo = modelOperationRepo;
        //     _stageRepo = stageRepo;
        //     _criticalRepo = criticalRepo;
        //     _configuration = configuration;
        //     _webHostEnvironment = webHostEnvironment;
        //     _mapper = mapper;
        //     _imageUrlUtility = imageUrlUtility;
        // }

        // public async Task<PagedList<BL_RolloutReportDTO>> GetAllRollout(BLSearchRolloutReportParam search, PaginationParams pagination)
        // {
        //     var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(search.Factory.Trim());
        //     _configuration.GetSection("AppSettings:DataSeach").Value = search.Factory.Trim();

        //     var predRollout = PredicateBuilder.New<BL_Rollout_Progress>(true);
        //     var predModel = PredicateBuilder.New<Model>(true);

        //     if (!string.IsNullOrEmpty(search.Factory))
        //         predRollout = predRollout.And(x => x.factory_id.Trim() == search.Factory.Trim());
        //     if (!string.IsNullOrEmpty(search.LineID))
        //         predRollout = predRollout.And(x => x.line_id.Trim() == search.LineID.Trim());
        //     if (!string.IsNullOrEmpty(search.LineTypeID))
        //         predRollout = predRollout.And(x => x.line_type_id.Trim() == search.LineTypeID.Trim());
        //     if (!string.IsNullOrEmpty(search.StageID))
        //         predRollout = predRollout.And(x => x.stage_id.Trim() == search.StageID.Trim());

        //     var rolloutQuery = _rolloutReportRepo.FindAll(predRollout).Select(x => new
        //     {
        //         x.factory_id,
        //         x.line_id,
        //         x.line_type_id,
        //         x.model_no,
        //         x.stage_id,
        //         x.rollout_line_id,
        //         x.rollout_date,
        //         x.mp_allocated,
        //         x.machines_qty,
        //         x.tool_qty,
        //         x.operation_id,
        //         x.operation_descriptions,
        //         x.operation_video_url,
        //         x.rollout_operation_layout,
        //     });

        //     var lineQuery = _linesRepo.FindAll().Select(x => new
        //     {
        //         x.factory_id,
        //         x.line_id,
        //         x.line_name,
        //     });

        //     var modelQuery = _modelRepo.FindAll(predModel).Select(x => new
        //     {
        //         x.factory_id,
        //         x.model_no,
        //         x.model_name
        //     });

        //     var modelOperationQuery = _modelOperationRepo.FindAll()
        //         .Select(x => new
        //         {
        //             x.factory_id,
        //             x.model_no,
        //             x.stage_id,
        //             x.operation_id,
        //             x.operation_name_local,
        //             x.operation_name_en,
        //             x.operation_name_zh
        //         });

        //     var criticalQuery = _criticalRepo.FindAll().Select(x => new
        //     {
        //         x.factory_id,
        //         x.line_id,
        //         x.line_type_id,
        //         x.model_no,
        //         x.stage_id,
        //         x.takt_time,
        //         x.ct_after_sec,
        //         x.operation_id
        //     });

        //     var queryJoin = rolloutQuery
        //         .GroupJoin(lineQuery,
        //             x => new { x.factory_id, x.rollout_line_id },
        //             y => new { y.factory_id, rollout_line_id = y.line_id },
        //             (x, y) => new { Rollout = x, Lines = y })
        //         .SelectMany(s => s.Lines.DefaultIfEmpty(), (x, y) => new { x.Rollout, Line = y })
        //         .GroupJoin(modelQuery,
        //             x => new { x.Rollout.factory_id, x.Rollout.model_no },
        //             y => new { y.factory_id, y.model_no },
        //             (x, y) => new { x.Rollout, x.Line, Models = y })
        //         .SelectMany(s => s.Models.DefaultIfEmpty(), (x, y) => new { x.Rollout, x.Line, Model = y })
        //         .GroupJoin(modelOperationQuery,
        //             x => new
        //             {
        //                 x.Rollout.factory_id,
        //                 x.Rollout.model_no,
        //                 x.Rollout.stage_id,
        //                 x.Rollout.operation_id
        //             },
        //             y => new
        //             {
        //                 y.factory_id,
        //                 y.model_no,
        //                 y.stage_id,
        //                 y.operation_id
        //             },
        //             (x, y) => new { x.Rollout, x.Line, x.Model, ModelOperations = y })
        //         .SelectMany(s => s.ModelOperations.DefaultIfEmpty(),
        //             (x, y) => new { x.Rollout, x.Line, x.Model, ModelOperation = y })
        //         .GroupJoin(criticalQuery,
        //             x => new
        //             {
        //                 x.Rollout.factory_id,
        //                 x.Rollout.line_id,
        //                 x.Rollout.line_type_id,
        //                 x.Rollout.model_no,
        //                 x.Rollout.stage_id,
        //                 x.Rollout.operation_id
        //             },
        //             y => new
        //             {
        //                 y.factory_id,
        //                 y.line_id,
        //                 y.line_type_id,
        //                 y.model_no,
        //                 y.stage_id,
        //                 y.operation_id
        //             },
        //             (x, y) => new { x.Rollout, x.Line, x.Model, x.ModelOperation, Criticals = y })
        //         .SelectMany(s => s.Criticals.DefaultIfEmpty(), (x, y) => new BL_RolloutReportDTO
        //         {
        //             line_name = x.Line.line_name,
        //             rollout_date = x.Rollout.rollout_date,
        //             model_no = x.Rollout.model_no,
        //             model_name = x.Model.model_name,
        //             stage_id = x.Rollout.stage_id,
        //             operation_name_local = x.ModelOperation.operation_name_local,
        //             operation_name_en = x.ModelOperation.operation_name_en,
        //             operation_name_zh = x.ModelOperation.operation_name_zh,
        //             takt_time = y.takt_time,
        //             ct_after_sec = y.ct_after_sec,
        //             mp_allocated = x.Rollout.mp_allocated,
        //             hourly_output = String.Format("{0:0.##}", x.Rollout.mp_allocated * (y.ct_after_sec == 0 ? 0 : (3600 / y.ct_after_sec))),
        //             machines_qty = x.Rollout.machines_qty,
        //             tool_qty = x.Rollout.tool_qty,
        //             operation_descriptions = x.Rollout.operation_descriptions,
        //             operation_video_url = x.Rollout.operation_video_url,
        //             rollout_operation_layout = x.Rollout.rollout_operation_layout,
        //             operation_video_url_result = imageUrl + x.Rollout.operation_video_url,
        //             rollout_operation_layout_result = imageUrl + x.Rollout.rollout_operation_layout,
        //         });

        //     if (!string.IsNullOrEmpty(search.ModelNo))
        //     {
        //         var searchModel = search.ModelNo.Trim().ToUpper();
        //         queryJoin = queryJoin.Where(x => x.model_no.Trim().ToUpper().Contains(searchModel) ||
        //             x.model_name.Trim().ToUpper().Contains(searchModel));
        //     }
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     var result = await PagedList<BL_RolloutReportDTO>.CreateAsync(queryJoin, pagination.PageNumber, pagination.PageSize);
        //     //Count Total
        //     var critical_operations_total = await criticalQuery.Where(x => x.line_id == search.LineID && x.line_type_id == search.LineTypeID).CountAsync();
        //     var criticalOperationsRolloutTotal = await rolloutQuery
        //         .GroupBy(x => new { x.factory_id, x.line_id, x.line_type_id, x.model_no, x.stage_id, x.operation_id })
        //         .CountAsync();
        //     if (result.Count > 0)
        //     {
        //         if (critical_operations_total > 0)
        //         {
        //             result[0].countBeingAudit = criticalOperationsRolloutTotal;
        //         }
        //         if (criticalOperationsRolloutTotal > 0)
        //         {
        //             result[0].criticalOperationsTotal = critical_operations_total;
        //         }
        //     }
        //     return result;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetFactory()
        // {
        //     var result = await _factoryRepo.FindAll()
        //         .Select(x => new KeyValuePair<string, string>(
        //             x.factory_id.Trim(),
        //             x.factory_name.Trim()
        //         )).ToListAsync();
        //     return result;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetLineNo(string factory)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
        //     // var result = await _linesRepo.FindAll().OrderBy(x => x.sequence)
        //     //     .Select(x => new KeyValuePair<string, string>(
        //     //         x.line_id.Trim(),
        //     //         x.line_name.Trim()
        //     //     )).Distinct().ToListAsync();
        //     var result = await _rolloutReportRepo.FindAll()
        //             .Join(_linesRepo.FindAll(),
        //             x => new { x.factory_id, x.line_id },
        //             y => new { y.factory_id, y.line_id },
        //             (x, y) => new { RolloutProgress = x, Lines = y })
        //             .Select(x => new KeyValuePair<string, string>(
        //             x.RolloutProgress.line_id.Trim(),
        //             x.Lines.line_name.Trim()
        //         )).Distinct().ToListAsync();

        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return result;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetLineType(string factory)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
        //     var result = await _lineTypeRepo.FindAll().OrderBy(x => x.sequence)
        //         .Select(x => new KeyValuePair<string, string>(
        //             x.line_type_id.Trim(),
        //             x.line_type_name.Trim()
        //         )).Distinct().ToListAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return result;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetStage(string factory)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
        //     var result = await _stageRepo.FindAll(x => x.is_active == true)
        //         .Distinct().OrderBy(x => x.sequence)
        //         .Select(x => new KeyValuePair<string, string>(
        //             x.stage_id.Trim(),
        //             x.stage_name.Trim()
        //         )).ToListAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return result;
        // }

    }
}