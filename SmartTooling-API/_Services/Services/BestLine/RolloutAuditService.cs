using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Models.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class RolloutAuditService : IRolloutAuditService
    {
        private readonly IBLLinesRepository _BLLinesRepository;
        private readonly IBLCriticalProcessAnalysisRepository _BLCriticalProcessAnalysisRepository;
        private readonly IBLLineTypeRepository _BLLineTypeRepository;
        private readonly IBLRolloutAuditRepository _BLRolloutAuditRepository;
        private readonly IBLRolloutProgressRepository _BLRolloutProgressRepository;
        private readonly IStageRepository _StageRepository;
        private readonly IModelOperationRepository _ModelOperationRepository;
        private readonly IModelRepository _ModelRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IConfiguration _configuration;
        private OperationResult operationResult;

        public RolloutAuditService(IBLLinesRepository bLLinesRepository,
            IBLCriticalProcessAnalysisRepository bLCriticalProcessAnalysisRepository,
            IBLLineTypeRepository bLLineTypeRepository,
            IBLRolloutAuditRepository bLRolloutAuditRepository,
            IBLRolloutProgressRepository bLRolloutProgressRepository,
            IStageRepository stageRepository,
            IModelOperationRepository modelOperationRepository,
            IModelRepository modelRepository, IMapper mapper,
            MapperConfiguration mapperConfiguration,
            IConfiguration configuration)
        {
            _BLLinesRepository = bLLinesRepository;
            _BLCriticalProcessAnalysisRepository = bLCriticalProcessAnalysisRepository;
            _BLLineTypeRepository = bLLineTypeRepository;
            _BLRolloutAuditRepository = bLRolloutAuditRepository;
            _BLRolloutProgressRepository = bLRolloutProgressRepository;
            _StageRepository = stageRepository;
            _ModelOperationRepository = modelOperationRepository;
            _ModelRepository = modelRepository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _configuration = configuration;
        }



        // public async Task<OperationResult> AddNew(BL_Rollout_AuditDTO bl_rollout_audit)
        // {
        //     bl_rollout_audit.audit_date = Convert.ToDateTime(bl_rollout_audit.audit_date_today + " 23:59");
        //     var models = _mapper.Map<BL_Rollout_Audit>(bl_rollout_audit);
        //     if (await checkBLRollout(bl_rollout_audit))
        //     {
        //         _BLRolloutAuditRepository.Add(models);
        //         try
        //         {
        //             await _BLRolloutAuditRepository.SaveAll();
        //             return operationResult = new OperationResult { Caption = "Success", Message = "Add This Bl Rollout Audit Succeeded", Success = true };
        //         }
        //         catch (System.Exception)
        //         {
        //             return operationResult = new OperationResult { Caption = "Fail", Message = "Add This Bl Rollout Audit fail!", Success = false };
        //         }
        //     }
        //     else
        //     {
        //         return operationResult = new OperationResult { Caption = "Fail", Message = "This Bl Rollout Audit is Exist", Success = false };
        //     }
        // }

        // public async Task<bool> checkBLRollout(BL_Rollout_AuditDTO parms_check)
        // {
        //     parms_check.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
        //     var data = await _BLRolloutAuditRepository
        //         .FindAll(x => x.rollout_line_id == parms_check.rollout_line_id
        //             && x.line_type_id == parms_check.line_type_id
        //             && x.model_no == parms_check.model_no
        //             && x.stage_id == parms_check.stage_id
        //             && x.operation_id == parms_check.operation_id
        //             && x.audit_count == parms_check.audit_count
        //             && x.factory_id == parms_check.factory_id).AsNoTracking()
        //         .FirstOrDefaultAsync();
        //     if (data == null)
        //     {
        //         return true;
        //     }
        //     else
        //     {
        //         return false;
        //     }
        // }

        // public async Task<OperationResult> Edit(BL_Rollout_AuditDTO bl_rollout_audit)
        // {
        //     if (await checkBLRollout(bl_rollout_audit))
        //     {
        //         return operationResult = new OperationResult { Caption = "Fail", Message = "This Bl Rollout Audit is not Exist", Success = false };
        //     }
        //     bl_rollout_audit.audit_date = Convert.ToDateTime(bl_rollout_audit.audit_date_today + " 23:59");
        //     // map and add
        //     var models = _mapper.Map<BL_Rollout_Audit>(bl_rollout_audit);
        //     _BLRolloutAuditRepository.Update(models);
        //     try
        //     {
        //         await _BLRolloutAuditRepository.SaveAll();
        //         return operationResult = new OperationResult { Caption = "Success", Message = "Edit This Bl Rollout Audit Succeeded", Success = true };
        //     }
        //     catch (System.Exception)
        //     {
        //         return operationResult = new OperationResult { Caption = "Fail", Message = "Edit This Bl Rollout Audit fail!", Success = false };
        //     }
        // }

        // public async Task<int> GetAuditCount(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var dataBL_r_audit = await _BLRolloutAuditRepository.FindAll(
        //             x => x.rollout_line_id == parms_get.rollout_line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no
        //             && x.stage_id == parms_get.stage_id
        //             && x.operation_id == parms_get.operation_id).Select(x => x.audit_count).ToListAsync();
        //     if (dataBL_r_audit.Count != 0)
        //     {
        //         var data = _BLRolloutAuditRepository.FindAll(
        //             x => x.rollout_line_id == parms_get.rollout_line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no
        //             && x.stage_id == parms_get.stage_id
        //             && x.operation_id == parms_get.operation_id).Max(x => x.audit_count) + 1;
        //         return await Task.FromResult(data);
        //     }
        //     else
        //     {
        //         return await Task.FromResult(1);
        //     }
        // }

        // public async Task<BL_Rollout_AuditDTO> GetBL_Rollout_Audt(Params_Rollout_ProgressDTO prams_get, int audit_count)
        // {
        //     var data = await _BLRolloutAuditRepository
        //         .FindAll(x => x.rollout_line_id == prams_get.rollout_line_id
        //             && x.line_type_id == prams_get.line_type_id
        //             && x.model_no == prams_get.model_no
        //             && x.stage_id == prams_get.stage_id
        //             && x.operation_id == prams_get.operation_id
        //             && x.audit_count == audit_count)
        //         .ProjectTo<BL_Rollout_AuditDTO>(_mapperConfiguration).FirstOrDefaultAsync();
        //     return data;
        // }

        // public async Task<List<BL_LinesDTO>> GetLineNo()
        // {
        //     var datablRollout = _BLRolloutProgressRepository.FindAll();
        //     var datalines = _BLLinesRepository.FindAll();
        //     var data = await datablRollout.GroupJoin(datalines,
        //         T1 => new { T1.factory_id, line_id = T1.rollout_line_id },
        //         T2 => new { T2.factory_id, T2.line_id },
        //         (T1, T2) => new { a = T1, b = T2 }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .Select(x => new BL_LinesDTO
        //         {
        //             rollout_line_id = x.a.rollout_line_id,
        //             line_id = x.b.line_id,
        //             sequence = x.b.sequence,
        //         }).OrderBy(x => x.sequence).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<List<ModelDTO>> GetModelNo(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var dataBL_r = _BLRolloutProgressRepository
        //         .FindAll(x => x.rollout_line_id == parms_get.rollout_line_id
        //             && x.line_type_id == parms_get.line_type_id);
        //     var dataModel = _ModelRepository.FindAll();
        //     var data = await dataBL_r.GroupJoin(dataModel,
        //         T1 => new { T1.factory_id, T1.model_no },
        //         T2 => new { T2.factory_id, T2.model_no },
        //         (T1, T2) => new { a = T1, b = T2 }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .Select(x => new ModelDTO
        //         {
        //             model_no = x.a.model_no,
        //             model_name = x.b.model_name,
        //         }).OrderBy(x => x.model_no).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<List<ModelOperationDTO>> GetOperaName(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var data_BL_r = _BLRolloutProgressRepository.FindAll(
        //             x => x.rollout_line_id == parms_get.rollout_line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no
        //             && x.stage_id == parms_get.stage_id);
        //     var dataModelOpera = _ModelOperationRepository.FindAll();
        //     var data = await data_BL_r.GroupJoin(dataModelOpera,
        //         T1 => new { T1.factory_id, T1.model_no, T1.stage_id, T1.operation_id },
        //         T2 => new { T2.factory_id, T2.model_no, T2.stage_id, T2.operation_id },
        //         (T1, T2) => new { a = T1, b = T2 }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .Select(x => new ModelOperationDTO
        //         {
        //             operation_id = x.a.operation_id,
        //             operation_name_zh = x.b != null ? x.b.operation_name_zh: null,
        //             operation_name_en = x.b != null ? x.b.operation_name_en: null,
        //             operation_name_local = x.b != null ? x.b.operation_name_local: null,
        //             sequence = x.b != null ? x.b.sequence : 0,
        //         }).OrderBy(x => x.sequence).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<BL_Critical_Process_AnalysisDTO> GetRemarks(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var data_BL_r = _BLRolloutProgressRepository.FindAll(
        //             x => x.rollout_line_id == parms_get.rollout_line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no
        //             && x.stage_id == parms_get.stage_id
        //             && x.operation_id == parms_get.operation_id);
        //     var data_BL_critical = _BLCriticalProcessAnalysisRepository.FindAll();
        //     var data = await data_BL_r.GroupJoin(data_BL_critical,
        //         T1 => new { T1.factory_id, T1.line_id, T1.model_no, T1.stage_id, T1.operation_id },
        //         T2 => new { T2.factory_id, T2.line_id, T2.model_no, T2.stage_id, T2.operation_id },
        //         (T1, T2) => new { a = T1, b = T2 }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .Select(x => new BL_Critical_Process_AnalysisDTO
        //         {
        //             man_remarks = x.b.man_remarks,
        //             machine_remarks = x.b.machine_remarks,
        //             method_remarks = x.b.method_remarks,
        //             material_remarks = x.b.material_remarks,
        //         }).FirstOrDefaultAsync();
        //     return data;
        // }

        // public async Task<List<StageDTO>> GetStage(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var data_BL_r = _BLRolloutProgressRepository.FindAll(
        //             x => x.rollout_line_id == parms_get.rollout_line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no);
        //     var dataStage = _StageRepository.FindAll();
        //     var data = await data_BL_r.GroupJoin(dataStage,
        //         T1 => new { T1.factory_id, T1.stage_id },
        //         T2 => new { T2.factory_id, T2.stage_id },
        //         (T1, T2) => new { a = T1, b = T2 }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .Select(x => new StageDTO
        //         {
        //             stage_id = x.a.stage_id,
        //             stage_name = x.b.stage_name,
        //             sequence = x.b.sequence,
        //         }).OrderBy(x => x.sequence).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<List<BL_Line_TypeDTO>> GetType(string rollout_line_id)
        // {
        //     var datablRollout = _BLRolloutProgressRepository.FindAll(x => x.rollout_line_id == rollout_line_id);
        //     var datalinesType = _BLLineTypeRepository.FindAll();
        //     var data = await datablRollout.GroupJoin(datalinesType,
        //         T1 => new { T1.factory_id, T1.line_type_id },
        //         T2 => new { T2.factory_id, T2.line_type_id },
        //         (T1, T2) => new { a = T1, b = T2 }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .Select(x => new BL_Line_TypeDTO
        //         {
        //             line_type_id = x.a.line_type_id,
        //             line_type_name = x.b.line_type_name,
        //             sequence = x.b.sequence,
        //         }).OrderBy(x => x.sequence).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<PageListUtility<BL_Rollout_AuditDTO>> Search(string rollout_line_id, string line_type_id, string text, PaginationParams param)
        // {
        //     var paramsSearch = PredicateBuilder.New<BL_Rollout_Audit>(true);
        //     if (!string.IsNullOrEmpty(rollout_line_id))
        //     {
        //         paramsSearch.And(x => x.rollout_line_id.Trim() == rollout_line_id.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(line_type_id))
        //     {
        //         paramsSearch.And(x => x.line_type_id.Trim() == line_type_id.Trim());
        //     }

        //     var dataBL_Rollout = _BLRolloutAuditRepository.FindAll(paramsSearch);
        //     var dataBL_Line = _BLLinesRepository.FindAll();
        //     var dataBL_Line_Type = _BLLineTypeRepository.FindAll();
        //     var dataModel = _ModelRepository.FindAll();
        //     var dataStage = _StageRepository.FindAll();
        //     var dataModel_Opera = _ModelOperationRepository.FindAll();
        //     var data = dataBL_Rollout.GroupJoin(dataBL_Line,
        //         x => new { x.factory_id, x.rollout_line_id },
        //         y => new { y.factory_id, rollout_line_id = y.line_id },
        //         (x, y) => new { a = x, b = y })
        //         .SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .GroupJoin(dataBL_Line_Type,
        //             x => new { x.a.factory_id, x.a.line_type_id },
        //             y => new { y.factory_id, y.line_type_id },
        //             (x, y) => new { x.a, x.b, c = y })
        //             .SelectMany(x => x.c.DefaultIfEmpty(), (x, y) => new { x.a, x.b, c = y })
        //             .GroupJoin(dataModel,
        //                 x => new { x.a.factory_id, x.a.model_no },
        //                 y => new { y.factory_id, y.model_no },
        //                 (x, y) => new { x.a, x.b, x.c, d = y })
        //                 .SelectMany(x => x.d.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, d = y })
        //                 .GroupJoin(dataStage,
        //                     x => new { x.a.factory_id, x.a.stage_id },
        //                     y => new { y.factory_id, y.stage_id },
        //                     (x, y) => new { x.a, x.b, x.c, x.d, e = y })
        //                     .SelectMany(x => x.e.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, x.d, e = y })
        //                     .GroupJoin(dataModel_Opera,
        //                         x => new { x.a.factory_id, x.a.model_no, x.a.stage_id, x.a.operation_id },
        //                         y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
        //                         (x, y) => new { x.a, x.b, x.c, x.d, x.e, f = y })
        //                         .SelectMany(x => x.f.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, x.d, x.e, f = y })
        //                         .Select(x => new BL_Rollout_AuditDTO
        //                         {
        //                             factory_id = x.a.factory_id,
        //                             rollout_line_id = x.a.rollout_line_id,
        //                             line_type_id = x.a.line_type_id,
        //                             stage_id = x.a.stage_id,
        //                             operation_id = x.a.operation_id,
        //                             audit_date = x.a.audit_date,
        //                             line_id = x.b.line_id,
        //                             line_name = x.b.line_name,
        //                             line_type_name = x.c.line_type_name,
        //                             audit_count = x.a.audit_count,
        //                             model_no = x.a.model_no,
        //                             model_name = x.d.model_name,
        //                             stage_name = x.e.stage_name,
        //                             operation_name_zh = x.f.operation_name_zh,
        //                             operation_name_en = x.f.operation_name_en,
        //                             operation_name_local = x.f.operation_name_local,
        //                             audit_result_line_is_pass = x.a.audit_result_line_is_pass,
        //                             audit_result_operation_is_pass = x.a.audit_result_operation_is_pass,
        //                         });
        //     if (!string.IsNullOrEmpty(text))
        //     {
        //         data = data.Where(x => x.model_no.ToUpper().Contains(text.ToUpper()) || x.model_name.ToUpper().Contains(text.ToUpper()));
        //     }
        //     return await PageListUtility<BL_Rollout_AuditDTO>.PageListAsync(data, param.PageNumber);

        // }

    }
}