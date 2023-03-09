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
    public class RolloutProgressService : IRolloutProgressService
    {
        private readonly IBLLinesRepository _BLLinesRepository;
        private readonly IBLLineTypeRepository _BLLineTypeRepository;
        private readonly IBLCriticalProcessAnalysisRepository _BLCriticalProcessAnalysisRepository;
        private readonly IBLRolloutProgressRepository _BLRolloutProgressRepository;
        private readonly IStageRepository _StageRepository;
        private readonly IModelOperationRepository _ModelOperationRepository;
        private readonly IModelRepository _ModelRepository;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IConfiguration _configuration;
        private OperationResult operationResult;

        public RolloutProgressService(IBLLinesRepository bLLinesRepository, IBLLineTypeRepository bLLineTypeRepository,
            IBLCriticalProcessAnalysisRepository bLCriticalProcessAnalysisRepository,
            IBLRolloutProgressRepository bLRolloutProgressRepository,
            IStageRepository stageRepository, IModelOperationRepository modelOperationRepository,
            IModelRepository modelRepository, IMapper mapper,
            MapperConfiguration mapperConfiguration, IConfiguration configuration)
        {
            _BLLinesRepository = bLLinesRepository;
            _BLLineTypeRepository = bLLineTypeRepository;
            _BLCriticalProcessAnalysisRepository = bLCriticalProcessAnalysisRepository;
            _BLRolloutProgressRepository = bLRolloutProgressRepository;
            _StageRepository = stageRepository;
            _ModelOperationRepository = modelOperationRepository;
            _ModelRepository = modelRepository;
            _mapper = mapper;
            _mapperConfiguration = mapperConfiguration;
            _configuration = configuration;
        }



        // public async Task<OperationResult> AddNew(BL_Rollout_ProgressDTO bl_rollout_progress)
        // {
        //     bl_rollout_progress.rollout_date = Convert.ToDateTime(bl_rollout_progress.rollout_date_convert + " 23:59");
        //     var models = _mapper.Map<BL_Rollout_Progress>(bl_rollout_progress);
        //     if (await checkBLRollout(bl_rollout_progress))
        //     {
        //         _BLRolloutProgressRepository.Add(models);
        //         await _BLRolloutProgressRepository.SaveAll();
        //         return operationResult = new OperationResult { Caption = "Success", Message = "Add This Bl Rollout Progress Succeeded", Success = true };
        //     }
        //     else
        //     {
        //         return operationResult = new OperationResult { Caption = "Exist", Message = "This Bl Rollout Progress is Exist", Success = false };
        //     }
        // }

        // public async Task<bool> checkBLRollout(BL_Rollout_ProgressDTO parms_check)
        // {
        //     parms_check.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
        //     var data = await _BLRolloutProgressRepository
        //         .FindAll(x => x.line_id == parms_check.line_id
        //             && x.line_type_id == parms_check.line_type_id
        //             && x.model_no == parms_check.model_no
        //             && x.stage_id == parms_check.stage_id
        //             && x.operation_id == parms_check.operation_id
        //             && x.rollout_line_id == parms_check.rollout_line_id
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

        // public async Task<BL_Rollout_ProgressDTO> GetBL_Rollout(Params_Rollout_ProgressDTO prams_get)
        // {
        //     var data = await _BLRolloutProgressRepository
        //         .FindAll(x => x.line_id == prams_get.line_id
        //             && x.line_type_id == prams_get.line_type_id
        //             && x.model_no == prams_get.model_no
        //             && x.stage_id == prams_get.stage_id
        //             && x.operation_id == prams_get.operation_id
        //             && x.rollout_line_id == prams_get.rollout_line_id)
        //         .ProjectTo<BL_Rollout_ProgressDTO>(_mapperConfiguration).FirstOrDefaultAsync();
        //     return data;
        // }

        // public async Task<decimal> GetCTAfter(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var data = await _BLCriticalProcessAnalysisRepository
        //         .FindAll(x => x.line_id == parms_get.line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no
        //             && x.stage_id == parms_get.stage_id
        //             && x.operation_id == parms_get.operation_id)
        //         .Select(x => x.ct_after_sec).FirstOrDefaultAsync();
        //     return data;
        // }

        // public async Task<BL_RolloutHourlyPPHDTO> GetHourlyPPH(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var data = await _BLCriticalProcessAnalysisRepository
        //         .FindAll(x => x.line_id == parms_get.line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no
        //             && x.stage_id == parms_get.stage_id
        //             && x.operation_id == parms_get.operation_id)
        //         .Select(x => new BL_RolloutHourlyPPHDTO
        //         {
        //             hourly_output = x.ct_after_sec == 0 ? 0 : Math.Round((3600 / x.ct_after_sec), 2),
        //             pph = x.ct_after_sec == 0 ? 0 : 3600 / x.ct_after_sec,
        //         })
        //         .FirstOrDefaultAsync();
        //     return data;
        // }

        // public async Task<List<BL_LinesDTO>> GetLineNo()
        // {
        //     var datalines = _BLLinesRepository.FindAll();
        //     var dataCritical = _BLCriticalProcessAnalysisRepository.FindAll();
        //     var data = await dataCritical.GroupJoin(datalines,
        //         T1 => new { T1.factory_id, T1.line_id },
        //         T2 => new { T2.factory_id, T2.line_id },
        //         (T1, T2) => new { a = T1, b = T2 }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .Select(x => new BL_LinesDTO
        //         {
        //             line_id = x.a.line_id,
        //             line_name = x.b.line_name,
        //             sequence = x.b.sequence,
        //         }).OrderBy(x => x.sequence).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<List<ModelDTO>> GetModelNo(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var dataCritical = _BLCriticalProcessAnalysisRepository
        //         .FindAll(x => x.line_id == parms_get.line_id
        //             && x.line_type_id == parms_get.line_type_id);
        //     var dataModel = _ModelRepository.FindAll();
        //     var data = await dataCritical.GroupJoin(dataModel,
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
        //     var dataCritical = _BLCriticalProcessAnalysisRepository.FindAll(
        //             x => x.line_id == parms_get.line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no
        //             && x.stage_id == parms_get.stage_id);
        //     var dataModelOpera = _ModelOperationRepository.FindAll();
        //     var data = await dataCritical.GroupJoin(dataModelOpera,
        //         T1 => new { T1.factory_id, T1.model_no, T1.stage_id, T1.operation_id },
        //         T2 => new { T2.factory_id, T2.model_no, T2.stage_id, T2.operation_id },
        //         (T1, T2) => new { a = T1, b = T2 }).SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .Select(x => new ModelOperationDTO
        //         {
        //             operation_id = x.a.operation_id,
        //             operation_name_zh = x.b != null ? x.b.operation_name_zh : null,
        //             operation_name_en = x.b != null ? x.b.operation_name_en : null,
        //             operation_name_local = x.b != null ? x.b.operation_name_local : null,
        //             sequence = x.b != null ? x.b.sequence : 0,
        //         }).OrderBy(x => x.sequence).Distinct().ToListAsync();
        //     return data;
        // }

        // public async Task<List<BL_LinesDTO>> GetRolloutLine()
        // {
        //     var data = await _BLLinesRepository.FindAll().ProjectTo<BL_LinesDTO>(_mapperConfiguration).ToListAsync();
        //     return data;
        // }

        // public async Task<BL_LinesDTO> GetRolloutLineNam(string line_id)
        // {
        //     var data = await _BLLinesRepository.FindAll(x => x.line_id == line_id)
        //         .ProjectTo<BL_LinesDTO>(_mapperConfiguration).FirstOrDefaultAsync();
        //     return data;
        // }

        // public async Task<List<StageDTO>> GetStage(Params_Rollout_ProgressDTO parms_get)
        // {
        //     var dataCritical = _BLCriticalProcessAnalysisRepository.FindAll(
        //             x => x.line_id == parms_get.line_id
        //             && x.line_type_id == parms_get.line_type_id
        //             && x.model_no == parms_get.model_no);
        //     var dataStage = _StageRepository.FindAll();
        //     var data = await dataCritical.GroupJoin(dataStage,
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

        // public async Task<List<BL_Line_TypeDTO>> GetType(string id)
        // {
        //     var datalinesType = _BLLineTypeRepository.FindAll();
        //     var dataCritical = _BLCriticalProcessAnalysisRepository.FindAll(x => x.line_id == id);
        //     var data = await dataCritical.GroupJoin(datalinesType,
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

        // public async Task<PageListUtility<BL_Rollout_ProgressDTO>> Search(string lineNo, string lineType, string model, PaginationParams param)
        // {
        //     var bl_critical = PredicateBuilder.New<BL_Rollout_Progress>(true);
        //     if (!string.IsNullOrEmpty(lineNo))
        //     {
        //         bl_critical.And(x => x.line_id.Trim() == lineNo.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(lineType))
        //     {
        //         bl_critical.And(x => x.line_type_id.Trim() == lineType.Trim());
        //     }
        //     if (!string.IsNullOrEmpty(model))
        //     {
        //         bl_critical.And(x => x.model_no.ToUpper().Contains(model.ToUpper()));
        //     }
        //     var dataBL_line_type = _BLLineTypeRepository.FindAll();
        //     var dataBL_Rollout = _BLRolloutProgressRepository.FindAll(bl_critical);
        //     var dataBL_Line = _BLLinesRepository.FindAll();
        //     var dataStage = _StageRepository.FindAll();
        //     var dataOpera = _ModelOperationRepository.FindAll();
        //     var dataBl_Critical = _BLCriticalProcessAnalysisRepository.FindAll();
        //     var data = dataBL_Rollout.GroupJoin(dataBL_Line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { a = x, b = y })
        //         .SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //         .GroupJoin(dataStage,
        //             x => new { x.a.factory_id, x.a.stage_id },
        //             y => new { y.factory_id, y.stage_id },
        //             (x, y) => new { x.a, x.b, c = y })
        //             .SelectMany(x => x.c.DefaultIfEmpty(), (x, y) => new { x.a, x.b, c = y })
        //             .GroupJoin(dataOpera,
        //                 x => new { x.a.factory_id, x.a.model_no, x.a.stage_id, x.a.operation_id },
        //                 y => new { y.factory_id, y.model_no, y.stage_id, y.operation_id },
        //                 (x, y) => new { x.a, x.b, x.c, d = y })
        //                 .SelectMany(x => x.d.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, d = y })
        //                 .GroupJoin(dataBl_Critical,
        //                     x => new { x.a.factory_id, x.a.line_id, x.a.line_type_id, x.a.model_no, x.a.stage_id, x.a.operation_id },
        //                     y => new { y.factory_id, y.line_id, y.line_type_id, y.model_no, y.stage_id, y.operation_id },
        //                     (x, y) => new { x.a, x.b, x.c, x.d, e = y })
        //                     .SelectMany(x => x.e.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, x.d, e = y })
        //                     .Select(x => new BL_Rollout_ProgressDTO
        //                     {
        //                         factory_id = x.a.factory_id,
        //                         rollout_date = x.a.rollout_date,
        //                         line_id = x.a.line_id,
        //                         line_type_id = x.a.line_type_id,
        //                         line_type_name = dataBL_line_type.Where(y => y.line_type_id == x.a.line_type_id).Select(y => y.line_type_name).FirstOrDefault(),
        //                         line_name = x.b.line_name,
        //                         rollout_line_name = dataBL_Line.Where(y => y.line_id == x.a.rollout_line_id).Select(y => y.line_id).FirstOrDefault(),
        //                         model_no = x.a.model_no,
        //                         stage_id = x.a.stage_id,
        //                         stage_name = x.c.stage_name,
        //                         operation_name_zh = x.d.operation_name_zh,
        //                         operation_name_en = x.d.operation_name_en,
        //                         operation_name_local = x.d.operation_name_local,
        //                         machines_name = x.a.machines_name,
        //                         machines_qty = x.a.machines_qty,
        //                         tool_name = x.a.tool_name,
        //                         tool_qty = x.a.tool_qty,
        //                         takt_time = x.e.takt_time,
        //                         ct_after_sec = x.e.ct_after_sec,
        //                         hourly_output = x.e.ct_after_sec == 0 ? 0 : (3600 / x.e.ct_after_sec) * x.a.mp_allocated,
        //                         pph = x.e.ct_after_sec == 0 ? 0 : (3600 / x.e.ct_after_sec),
        //                         operation_descriptions = x.a.operation_descriptions,
        //                         operation_id = x.a.operation_id,
        //                         rollout_line_id = x.a.rollout_line_id,
        //                         update_by = x.a.update_by,
        //                         update_time = x.a.update_time
        //                     });
        //     return await PageListUtility<BL_Rollout_ProgressDTO>.PageListAsync(data, param.PageNumber);
        // }

        // public async Task<OperationResult> Update(BL_Rollout_ProgressDTO bl_rollout_progress)
        // {
        //     if (await checkBLRollout(bl_rollout_progress))
        //     {
        //         return new OperationResult { Success = false, Message = "failed" };
        //     }
        //     //check rollout date
        //     if (!string.IsNullOrEmpty(bl_rollout_progress.rollout_date_convert))
        //     {
        //         bl_rollout_progress.rollout_date = Convert.ToDateTime(bl_rollout_progress.rollout_date_convert + " 23:59");
        //     }
        //     else
        //     {
        //         bl_rollout_progress.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
        //         var data = await _BLRolloutProgressRepository
        //             .FindAll(x => x.line_id == bl_rollout_progress.line_id
        //                 && x.line_type_id == bl_rollout_progress.line_type_id
        //                 && x.model_no == bl_rollout_progress.model_no
        //                 && x.stage_id == bl_rollout_progress.stage_id
        //                 && x.operation_id == bl_rollout_progress.operation_id
        //                 && x.rollout_line_id == bl_rollout_progress.rollout_line_id
        //                 && x.factory_id == bl_rollout_progress.factory_id).Select(x => x.rollout_date)
        //             .FirstOrDefaultAsync();
        //         bl_rollout_progress.rollout_date = data;
        //     }
        //     //check rollout operation layout
        //     if (string.IsNullOrEmpty(bl_rollout_progress.rollout_operation_layout))
        //     {
        //         bl_rollout_progress.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
        //         var data = await _BLRolloutProgressRepository
        //             .FindAll(x => x.line_id == bl_rollout_progress.line_id
        //                 && x.line_type_id == bl_rollout_progress.line_type_id
        //                 && x.model_no == bl_rollout_progress.model_no
        //                 && x.stage_id == bl_rollout_progress.stage_id
        //                 && x.operation_id == bl_rollout_progress.operation_id
        //                 && x.rollout_line_id == bl_rollout_progress.rollout_line_id
        //                 && x.factory_id == bl_rollout_progress.factory_id).Select(x => x.rollout_operation_layout).AsNoTracking()
        //             .FirstOrDefaultAsync();
        //         bl_rollout_progress.rollout_operation_layout = data;
        //     }
        //     //check operation video url
        //     if (string.IsNullOrEmpty(bl_rollout_progress.operation_video_url))
        //     {
        //         bl_rollout_progress.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
        //         var data = await _BLRolloutProgressRepository
        //             .FindAll(x => x.line_id == bl_rollout_progress.line_id
        //                 && x.line_type_id == bl_rollout_progress.line_type_id
        //                 && x.model_no == bl_rollout_progress.model_no
        //                 && x.stage_id == bl_rollout_progress.stage_id
        //                 && x.operation_id == bl_rollout_progress.operation_id
        //                 && x.rollout_line_id == bl_rollout_progress.rollout_line_id
        //                 && x.factory_id == bl_rollout_progress.factory_id).Select(x => x.operation_video_url).AsNoTracking()
        //             .FirstOrDefaultAsync();
        //         bl_rollout_progress.operation_video_url = data;
        //     }
        //     // keep create when update
        //     bl_rollout_progress.factory_id = _configuration.GetSection("AppSettings:Factory").Value;
        //     var dataUpdate = await _BLRolloutProgressRepository
        //         .FindAll(x => x.line_id == bl_rollout_progress.line_id
        //             && x.line_type_id == bl_rollout_progress.line_type_id
        //             && x.model_no == bl_rollout_progress.model_no
        //             && x.stage_id == bl_rollout_progress.stage_id
        //             && x.operation_id == bl_rollout_progress.operation_id
        //             && x.rollout_line_id == bl_rollout_progress.rollout_line_id
        //             && x.factory_id == bl_rollout_progress.factory_id).Select(x => new { x.create_by, x.create_time }).AsNoTracking()
        //         .FirstOrDefaultAsync();
        //     bl_rollout_progress.create_by = dataUpdate.create_by;
        //     bl_rollout_progress.create_time = dataUpdate.create_time;
        //     //map and add
        //     var model = _mapper.Map<BL_Rollout_Progress>(bl_rollout_progress);
        //     _BLRolloutProgressRepository.Update(model);
        //     try
        //     {
        //         await _BLRolloutProgressRepository.SaveAll();
        //         return new OperationResult { Success = true, Message = "This Bl Rollout Progress was successfully Update." };
        //     }
        //     catch (System.Exception)
        //     {
        //         return new OperationResult { Success = false, Message = "Updating This Bl Rollout Progress failed on save." };
        //     }
        // }

    }
}