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

namespace SmartTooling_API._Services.Services.BestLine
{
    public class BLAuditReportService : IBLAuditReportService
    {
        private readonly IBLRolloutProgressRepository _rolloutReportRepo;
        private readonly IBLRolloutAuditRepository _rolloutAuditRepo;
        private readonly IBLCriticalProcessAnalysisRepository _criticalRepo;
        private readonly IBLLayoutDesignOverallRepository _layoutDesignOverallRepo;
        private readonly IFactoryRepository _factoryRepo;
        private readonly IBLLinesRepository _linesRepo;
        private readonly IBLLineTypeRepository _lineTypeRepo;
        private readonly IModelRepository _modelRepo;
        private readonly IModelOperationRepository _modelOperationRepo;
        private readonly IStageRepository _stageRepo;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;
        private readonly IImageUrlUtility _imageUrlUtility;

        public BLAuditReportService(IBLRolloutProgressRepository rolloutReportRepo, IBLRolloutAuditRepository rolloutAuditRepo, IBLCriticalProcessAnalysisRepository criticalRepo, IBLLayoutDesignOverallRepository layoutDesignOverallRepo, IFactoryRepository factoryRepo, IBLLinesRepository linesRepo, IBLLineTypeRepository lineTypeRepo, IModelRepository modelRepo, IModelOperationRepository modelOperationRepo, IStageRepository stageRepo, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IMapper mapper, IImageUrlUtility imageUrlUtility)
        {
            _rolloutReportRepo = rolloutReportRepo;
            _rolloutAuditRepo = rolloutAuditRepo;
            _criticalRepo = criticalRepo;
            _layoutDesignOverallRepo = layoutDesignOverallRepo;
            _factoryRepo = factoryRepo;
            _linesRepo = linesRepo;
            _lineTypeRepo = lineTypeRepo;
            _modelRepo = modelRepo;
            _modelOperationRepo = modelOperationRepo;
            _stageRepo = stageRepo;
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
            _imageUrlUtility = imageUrlUtility;
        }


        // public async Task<PagedList<BL_AuditReportDTO>> GetAllAudit(BLSearchAuditReportParam search, PaginationParams pagination)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = search.Factory.Trim();

        //     var predRollout = PredicateBuilder.New<BL_Rollout_Progress>(true);
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
        //         x.operation_id,
        //     });

        //     var lineQuery = _linesRepo.FindAll().Select(x => new { x.factory_id, x.line_id, x.line_name });

        //     var modelQuery = _modelRepo.FindAll().Select(x => new { x.factory_id, x.model_no, x.model_name });

        //     var stageQuery = _stageRepo.FindAll().Select(x => new { x.factory_id, x.stage_id, x.stage_name });

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

        //     var auditQeury = _rolloutAuditRepo.FindAll()
        //         .Select(x => new
        //         {
        //             factory_id = x.factory_id.Trim(),
        //             rollout_line_id = x.rollout_line_id.Trim(),
        //             line_type_id = x.line_type_id.Trim(),
        //             model_no = x.model_no.Trim(),
        //             stage_id = x.stage_id.Trim(),
        //             operation_id = x.operation_id.Trim(),
        //             x.audit_count,
        //             x.audit_date,
        //             x.audit_result_line_is_pass,
        //             x.audit_result_operation_is_pass
        //         });

        //     var queryJoin = rolloutQuery.GroupJoin(lineQuery,
        //             x => new { x.factory_id, x.rollout_line_id },
        //             y => new { y.factory_id, rollout_line_id = y.line_id },
        //             (x, y) => new { Rollout = x, Lines = y })
        //         .SelectMany(s => s.Lines.DefaultIfEmpty(), (x, y) => new { x.Rollout, Line = y })
        //         .GroupJoin(modelQuery,
        //             x => new { x.Rollout.factory_id, x.Rollout.model_no },
        //             y => new { y.factory_id, y.model_no },
        //             (x, y) => new { x.Rollout, x.Line, Models = y })
        //         .SelectMany(s => s.Models.DefaultIfEmpty(), (x, y) => new { x.Rollout, x.Line, Model = y })
        //         .GroupJoin(stageQuery,
        //             x => new { x.Rollout.factory_id, x.Rollout.stage_id },
        //             y => new { y.factory_id, y.stage_id },
        //             (x, y) => new { x.Rollout, x.Line, x.Model, Stages = y })
        //         .SelectMany(s => s.Stages.DefaultIfEmpty(), (x, y) => new { x.Rollout, x.Line, x.Model, Stage = y })
        //         .GroupJoin(modelOperationQuery,
        //             x => new { x.Rollout.factory_id, x.Rollout.model_no, x.Rollout.stage_id, x.Rollout.operation_id },
        //             y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
        //             (x, y) => new { x.Rollout, x.Line, x.Model, x.Stage, ModelOperations = y })
        //         .SelectMany(s => s.ModelOperations.DefaultIfEmpty(), (x, y) => new BL_AuditReportDTO
        //         {
        //             factory_id = x.Rollout.factory_id,
        //             line_id = x.Rollout.line_id,
        //             line_name = x.Line.line_name,
        //             line_type_id = x.Rollout.line_type_id,
        //             model_no = x.Rollout.model_no,
        //             model_name = x.Model.model_name,
        //             stage_name = x.Stage.stage_name,
        //             stage_id = x.Stage.stage_id,
        //             rollout_line_id = x.Rollout.rollout_line_id,
        //             operation_id = y.operation_id,
        //             operation_name_local = y.operation_name_local,
        //             operation_name_en = y.operation_name_en,
        //             operation_name_zh = y.operation_name_zh,
        //             rollout_date = x.Rollout.rollout_date,
        //             first_audit_date = auditQeury
        //                     .Where(z =>
        //                         z.rollout_line_id == x.Rollout.rollout_line_id &&
        //                         z.line_type_id == x.Rollout.line_type_id &&
        //                         z.model_no == x.Rollout.model_no &&
        //                         z.stage_id == x.Rollout.stage_id &&
        //                         z.operation_id == x.Rollout.operation_id)
        //                     .OrderBy(y => y.audit_count).Select(x => x.audit_date).FirstOrDefault(),
        //             latest_audit_date = auditQeury
        //                     .Where(z =>
        //                         z.rollout_line_id == x.Rollout.rollout_line_id &&
        //                         z.line_type_id == x.Rollout.line_type_id &&
        //                         z.model_no == x.Rollout.model_no &&
        //                         z.stage_id == x.Rollout.stage_id &&
        //                         z.operation_id == x.Rollout.operation_id)
        //                     .OrderByDescending(y => y.audit_count).Select(x => x.audit_date).FirstOrDefault(),
        //             audit_conducted = auditQeury
        //                     .Where(z =>
        //                         z.rollout_line_id == x.Rollout.rollout_line_id &&
        //                         z.line_type_id == x.Rollout.line_type_id &&
        //                         z.model_no == x.Rollout.model_no &&
        //                         z.stage_id == x.Rollout.stage_id &&
        //                         z.operation_id == x.Rollout.operation_id)
        //                     .OrderByDescending(y => y.audit_count).Select(x => x.audit_count).FirstOrDefault(),
        //             layout_audit = auditQeury
        //                     .Where(z =>
        //                         z.rollout_line_id == x.Rollout.rollout_line_id &&
        //                         z.line_type_id == x.Rollout.line_type_id &&
        //                         z.model_no == x.Rollout.model_no &&
        //                         z.stage_id == x.Rollout.stage_id &&
        //                         z.operation_id == x.Rollout.operation_id)
        //                     .OrderByDescending(y => y.audit_count)
        //                     .Select(x => x.audit_result_line_is_pass).FirstOrDefault(),
        //             audit_pass = auditQeury
        //                     .Where(z =>
        //                         z.rollout_line_id == x.Rollout.rollout_line_id &&
        //                         z.line_type_id == x.Rollout.line_type_id &&
        //                         z.model_no == x.Rollout.model_no &&
        //                         z.stage_id == x.Rollout.stage_id &&
        //                         z.operation_id == x.Rollout.operation_id &&
        //                         z.audit_result_operation_is_pass == true).Count(),
        //             audit_fail = auditQeury
        //                     .Where(z =>
        //                         z.rollout_line_id == x.Rollout.rollout_line_id &&
        //                         z.line_type_id == x.Rollout.line_type_id &&
        //                         z.model_no == x.Rollout.model_no &&
        //                         z.stage_id == x.Rollout.stage_id &&
        //                         z.operation_id == x.Rollout.operation_id &&
        //                         z.audit_result_operation_is_pass == false).Count()
        //         });

        //     if (!string.IsNullOrEmpty(search.ModelNo))
        //     {
        //         var searchModel = search.ModelNo.Trim().ToUpper();
        //         queryJoin = queryJoin.Where(x => x.model_no.Trim().ToUpper().Contains(searchModel) ||
        //             x.model_name.Trim().ToUpper().Contains(searchModel));
        //     }
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     var result = await PagedList<BL_AuditReportDTO>.CreateAsync(queryJoin, pagination.PageNumber, pagination.PageSize);
        //     var countBeingAudit = await queryJoin.Where(x => x.audit_conducted > 0).CountAsync();
        //     if(await queryJoin.CountAsync() > 0) {
        //         result[0].countBeingAudit = countBeingAudit;
        //     }
        //     return result;
        // }

        // //>>>>>>>> Audit detail

        // public async Task<object> BestPracticeUrl(BLSearchAuditReportParam param)
        // {
        //     var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(param.Factory.Trim());
        //     _configuration.GetSection("AppSettings:DataSeach").Value = param.Factory.Trim();
        //     var result = await _criticalRepo
        //     .FindAll(x => x.line_id == param.LineID &&
        //         x.line_type_id == param.LineTypeID &&
        //         x.model_no == param.ModelNo &&
        //         x.stage_id == param.StageID &&
        //         x.operation_id == param.OperationID)
        //     .Select(x => x.best_practice_url).FirstOrDefaultAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return new { best_practice_url = imageUrl + result };
        // }

        // public async Task<object> LayoutLink(BLSearchAuditReportParam param)
        // {
        //     var imageUrl = await _imageUrlUtility.GetImageUrlByFactory(param.Factory.Trim());
        //     _configuration.GetSection("AppSettings:DataSeach").Value = param.Factory.Trim();
        //     var result = await _layoutDesignOverallRepo
        //     .FindAll(x => x.line_id == param.LineID &&
        //         x.line_type_id == param.LineTypeID &&
        //         x.model_no == param.ModelNo)
        //     .Select(x => x.c2b_overall_image).FirstOrDefaultAsync();
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return new { c2b_overall_image = imageUrl + result };
        // }

        // public async Task<PagedList<BL_AuditReportDetailDTO>> GetAuditDetail(BLSearchAuditReportParam search, PaginationParams pagination)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = search.Factory.Trim();
        //     var predAudit = PredicateBuilder.New<BL_Rollout_Audit>(true);

        //     if (!string.IsNullOrEmpty(search.Factory))
        //         predAudit = predAudit.And(x => x.factory_id.Trim() == search.Factory.Trim());
        //     if (!string.IsNullOrEmpty(search.LineID))
        //         predAudit = predAudit.And(x => x.rollout_line_id.Trim() == search.RolloutLineID.Trim());
        //     if (!string.IsNullOrEmpty(search.LineTypeID))
        //         predAudit = predAudit.And(x => x.line_type_id.Trim() == search.LineTypeID.Trim());
        //     if (!string.IsNullOrEmpty(search.ModelNo))
        //         predAudit = predAudit.And(x => x.model_no.Trim() == search.ModelNo.Trim());
        //     if (!string.IsNullOrEmpty(search.StageID))
        //         predAudit = predAudit.And(x => x.stage_id.Trim() == search.StageID.Trim());
        //     if (!string.IsNullOrEmpty(search.OperationID))
        //         predAudit = predAudit.And(x => x.operation_id.Trim() == search.OperationID.Trim());

        //     var auditQeury = _rolloutAuditRepo.FindAll(predAudit).Select(x => new
        //     {
        //         x.factory_id,
        //         x.line_type_id,
        //         x.rollout_line_id,
        //         x.model_no,
        //         x.stage_id,
        //         x.operation_id,
        //         x.audit_count,
        //         x.audit_date,
        //         x.audit_result_line_is_pass,
        //         x.audit_result_operation_is_pass,
        //         x.operation_gap_description
        //     });

        //     var lineQuery = _linesRepo.FindAll().Select(x => new
        //     {
        //         x.factory_id,
        //         x.line_id,
        //         x.line_name,
        //     });

        //     var lineTypeQuery = _lineTypeRepo.FindAll().Select(x => new
        //     {
        //         x.factory_id,
        //         x.line_type_id,
        //         x.line_type_name
        //     });

        //     var modelQuery = _modelRepo.FindAll().Select(x => new
        //     {
        //         x.factory_id,
        //         x.model_no,
        //         x.model_name
        //     });

        //     var stageQuery = _stageRepo.FindAll().Select(x => new
        //     {
        //         x.factory_id,
        //         x.stage_id,
        //         x.stage_name
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

        //     var queryJoin = auditQeury.GroupJoin(lineQuery,
        //             x => new { x.factory_id, x.rollout_line_id },
        //             y => new { y.factory_id, rollout_line_id = y.line_id },
        //             (x, y) => new { Audit = x, Lines = y })
        //         .SelectMany(s => s.Lines.DefaultIfEmpty(), (x, y) => new { x.Audit, Line = y })
        //         .GroupJoin(lineTypeQuery,
        //             x => x.Audit.line_type_id,
        //             y => y.line_type_id,
        //             (x, y) => new { x.Audit, x.Line, LineTypes = y })
        //         .SelectMany(s => s.LineTypes.DefaultIfEmpty(), (x, y) => new
        //         {
        //             x.Audit,
        //             x.Line,
        //             LineType = y
        //         }).GroupJoin(modelQuery,
        //             x => new { x.Audit.factory_id, x.Audit.model_no },
        //             y => new { y.factory_id, y.model_no },
        //             (x, y) => new { x.Audit, x.Line, x.LineType, Models = y })
        //         .SelectMany(s => s.Models.DefaultIfEmpty(), (x, y) => new
        //         {
        //             x.Audit,
        //             x.Line,
        //             x.LineType,
        //             Model = y
        //         }).GroupJoin(stageQuery,
        //             x => new { x.Audit.factory_id, x.Audit.stage_id },
        //             y => new { y.factory_id, y.stage_id },
        //             (x, y) => new { x.Audit, x.Line, x.LineType, x.Model, Stages = y })
        //         .SelectMany(s => s.Stages.DefaultIfEmpty(), (x, y) => new
        //         {
        //             x.Audit,
        //             x.Line,
        //             x.LineType,
        //             x.Model,
        //             Stage = y
        //         }).GroupJoin(modelOperationQuery,
        //             x => new { x.Audit.factory_id, x.Audit.model_no, x.Audit.stage_id, x.Audit.operation_id },
        //             y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
        //             (x, y) => new { x.Audit, x.Line, x.LineType, x.Model, x.Stage, ModelOperations = y })
        //         .SelectMany(s => s.ModelOperations.DefaultIfEmpty(), (x, y) => new BL_AuditReportDetailDTO
        //         {
        //             audit_date = x.Audit.audit_date,
        //             line_name = x.Line.line_name,
        //             line_type_name = x.LineType.line_type_name,
        //             audit_count = x.Audit.audit_count,
        //             model_no = x.Audit.model_no,
        //             model_name = x.Model.model_name,
        //             stage_name = x.Stage.stage_name,
        //             operation_name_local = y.operation_name_local,
        //             operation_name_en = y.operation_name_en,
        //             operation_name_zh = y.operation_name_zh,
        //             audit_result_line_is_pass = x.Audit.audit_result_line_is_pass,
        //             audit_result_operation_is_pass = x.Audit.audit_result_operation_is_pass,
        //             operation_gap_description = x.Audit.operation_gap_description
        //         }).OrderBy(x => x.audit_count);
        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     var result = await PagedList<BL_AuditReportDetailDTO>.CreateAsync(queryJoin, pagination.PageNumber, pagination.PageSize);
        //     return result;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetLineNo(string factory)
        // {
        //     _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
        //     var data = await _rolloutReportRepo.FindAll()
        //                 .Join(_rolloutAuditRepo.FindAll(x=>!string.IsNullOrWhiteSpace(x.rollout_line_id)),
        //                 x => new { x.factory_id, x.rollout_line_id, x.line_type_id, x.model_no, x.stage_id, x.operation_id },
        //                 y => new { y.factory_id, y.rollout_line_id, y.line_type_id, y.model_no, y.stage_id, y.operation_id },
        //                 (x, y) => new { Progress = x, Audit = y })
        //                 .Join(_linesRepo.FindAll().AsNoTracking(),
        //                 x=>x.Progress.line_id,
        //                 y=>y.line_id,
        //                 (x,y)=>new {x.Audit,x.Progress, Line = y})
        //                 .Select(x => new KeyValuePair<string, string>(
        //                     x.Line.line_id,
        //                     x.Line.line_name
        //                 )).Distinct().ToListAsync();

        //     _configuration.GetSection("AppSettings:DataSeach").Value = "";
        //     return data;
        // }
    }
}