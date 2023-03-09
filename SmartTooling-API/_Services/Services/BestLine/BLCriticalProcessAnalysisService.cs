using System.IO;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Models.BestLine;
using SmartTooling_API.DTO.SmartTool;
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using AutoMapper.QueryableExtensions;
using System.Security.Claims;
using SmartTooling_API.Helpers.Params.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class BLCriticalProcessAnalysisService : IBLCriticalProcessAnalysisService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webhostEnvironment;
        private readonly IBLCriticalProcessAnalysisRepository _iBLCriticalProcessAnalysisRepo;
        private readonly IBLLinesRepository _bLLinesRepo;
        private readonly IBLLineTypeRepository _bLLineTypeRepo;
        private readonly IBLLayoutDesignOverallRepository _bLLayoutDesignOverallRepo;
        private readonly IStageRepository _stageRepo;
        private readonly IModelOperationRepository _modelOperationRepo;
        private readonly IModelRepository _modelRepo;


        public BLCriticalProcessAnalysisService(IMapper mapper, MapperConfiguration configMapper,
        IBLCriticalProcessAnalysisRepository iBLCriticalProcessAnalysisRepo, IBLLinesRepository bLLinesRepo, IBLLineTypeRepository bLLineTypeRepo,
        IBLLayoutDesignOverallRepository bLLayoutDesignOverallRepo, IStageRepository stageRepo, IModelOperationRepository modelOperationRepo,
        IModelRepository modelRepo, IWebHostEnvironment webhostEnvironment, IConfiguration configuration
        )
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _iBLCriticalProcessAnalysisRepo = iBLCriticalProcessAnalysisRepo;
            _bLLayoutDesignOverallRepo = bLLayoutDesignOverallRepo;
            _bLLinesRepo = bLLinesRepo;
            _bLLineTypeRepo = bLLineTypeRepo;
            _modelOperationRepo = modelOperationRepo;
            _stageRepo = stageRepo;
            _modelRepo = modelRepo;
            _webhostEnvironment = webhostEnvironment;
            _configuration = configuration;
        }

        // public async Task<PageListUtility<BL_Critical_ProcessDTO>> GetAll(SearchBLCriticalParam search, PaginationParams pagination, bool isPaging = true)
        // {
        //     var predBas = PredicateBuilder.New<BL_Critical_Process_Analysis>(true);
        //     if (search.LineNo != null && search.LineNo != string.Empty)
        //     {
        //         predBas = predBas.And(x => x.line_id.Trim() == search.LineNo.Trim());
        //     }
        //     if (search.LineType != null && search.LineType != string.Empty)
        //     {
        //         predBas = predBas.And(x => x.line_type_id.Trim() == search.LineType.Trim());
        //     }
        //     if (search.Stage != null && search.Stage != string.Empty)
        //     {
        //         predBas = predBas.And(x => x.stage_id.Trim() == search.Stage.Trim());
        //     }

        //     if (!string.IsNullOrEmpty(search.ModelNo))
        //     {
        //         var modelNo = search.ModelNo.Trim().ToUpper();
        //         predBas = predBas.And(x => x.model_no.ToUpper().Contains(modelNo));
        //     }

        //     var operQuery = _modelOperationRepo.FindAll();
        //     var stageQuery = _stageRepo.FindAll();
        //     var lineQuery = _bLLinesRepo.FindAll();
        //     var lineTypeQuery = _bLLineTypeRepo.FindAll();
        //     var criticalProcessQuery = _iBLCriticalProcessAnalysisRepo.FindAll(predBas);

        //     var data = criticalProcessQuery
        //     .GroupJoin(lineQuery,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { a = x, b = y })
        //         .SelectMany(x => x.b.DefaultIfEmpty(), (x, y) => new { x.a, b = y })
        //     .GroupJoin(lineTypeQuery,
        //         x => new { x.a.factory_id, x.a.line_type_id },
        //         y => new { y.factory_id, y.line_type_id },
        //         (x, y) => new { x.a, x.b, c = y })
        //         .SelectMany(x => x.c.DefaultIfEmpty(), (x, y) => new { x.a, x.b, c = y })
        //         .GroupJoin(stageQuery,
        //             x => new { x.a.factory_id, x.a.stage_id },
        //             y => new { y.factory_id, y.stage_id },
        //             (x, y) => new { x.a, x.b, x.c, d = y })
        //             .SelectMany(x => x.d.DefaultIfEmpty(), (x, y) => new { x.a, x.b, x.c, d = y })
        //             .GroupJoin(operQuery,
        //                 x => new { x.a.factory_id, x.a.operation_id, x.a.model_no },
        //                 y => new { y.factory_id, y.operation_id, y.model_no },
        //                 (x, y) => new { x.a, x.b, x.c, x.d, e = y })
        //                 .SelectMany(x => x.e.DefaultIfEmpty(), (x, y) => new BL_Critical_ProcessDTO
        //                 {
        //                     line_name = x.b.line_name.Trim(),
        //                     line_id = x.b.line_id.Trim(),
        //                     factory_id = x.a.factory_id.Trim(),
        //                     line_type_id = x.c.line_type_id.Trim(),
        //                     stage_id = x.d.stage_id.Trim(),
        //                     line_type_name = x.c.line_type_name.Trim(),
        //                     model_no = x.a.model_no.Trim(),
        //                     stage_name = x.d.stage_name.Trim(),
        //                     operation_name_zh = y.operation_name_zh.Trim(),
        //                     operation_name_en = y.operation_name_en.Trim(),
        //                     operation_name_local = y.operation_name_local.Trim(),
        //                     best_practice_url = x.a.best_practice_url,
        //                     ct_after_sec = x.a.ct_after_sec,
        //                     ct_before_sec = x.a.ct_before_sec,
        //                     takt_time = x.a.takt_time,
        //                     man_media_url = x.a.man_media_url,
        //                     man_remarks = x.a.man_remarks,
        //                     method_media_url = x.a.method_media_url,
        //                     method_remarks = x.a.method_remarks,
        //                     material_media_url = x.a.material_media_url,
        //                     material_remarks = x.a.material_remarks,
        //                     machine_media_url = x.a.machine_media_url,
        //                     machine_remarks = x.a.machine_remarks,
        //                     operation_id = y.operation_id.Trim(),
        //                     update_by = x.a.update_by.Trim(),
        //                     update_time = x.a.update_time,
        //                 });

        //     return await PageListUtility<BL_Critical_ProcessDTO>
        //        .PageListAsync(data, pagination.PageNumber, pagination.PageSize, isPaging);
        // }

        // public async Task<bool> IsExists(BL_Critical_Process_AnalysisDTO bLCriticalDto)
        // {
        //     bLCriticalDto.factory_id = _configuration.GetSection("Appsettings:Factory").Value;
        //     var model = await _iBLCriticalProcessAnalysisRepo
        //         .FindAll(x => x.model_no.Trim() == bLCriticalDto.model_no.Trim()
        //             && x.line_id.Trim() == bLCriticalDto.line_id.Trim()
        //             && x.line_type_id.Trim() == bLCriticalDto.line_type_id.Trim()
        //             && x.operation_id.Trim() == bLCriticalDto.operation_id.Trim()
        //             && x.stage_id.Trim() == bLCriticalDto.stage_id.Trim()
        //             && x.factory_id.Trim() == bLCriticalDto.factory_id.Trim())
        //         .AsNoTracking().FirstOrDefaultAsync();
        //     return model == null ? false : true;
        // }

        // public async Task<OperationResult> Create(BL_Critical_Process_AnalysisDTO bLCritical)
        // {
        //     if (await IsExists(bLCritical) == true)
        //     {
        //         return new OperationResult { Success = false, Caption = "Critical Process Analysis already exists." };
        //     }
        //     var fn = new FunctionUtility();
        //     var bLCriticaItem = new BL_Critical_Process_Analysis();
        //     bLCriticaItem.factory_id = _configuration.GetSection("Appsettings:Factory").Value;
        //     bLCriticaItem.stage_id = bLCritical.stage_id;
        //     bLCriticaItem.line_id = bLCritical.line_id;
        //     bLCriticaItem.update_time = DateTime.Now;
        //     bLCriticaItem.operation_id = bLCritical.operation_id;
        //     bLCriticaItem.line_type_id = bLCritical.line_type_id;
        //     bLCriticaItem.model_no = bLCritical.model_no;
        //     bLCriticaItem.ct_after_sec = bLCritical.ct_after_sec;
        //     bLCriticaItem.ct_before_sec = bLCritical.ct_before_sec;
        //     bLCriticaItem.machine_remarks = bLCritical.machine_remarks;
        //     bLCriticaItem.man_remarks = bLCritical.man_remarks;
        //     bLCriticaItem.material_remarks = bLCritical.material_remarks;
        //     bLCriticaItem.method_remarks = bLCritical.method_remarks;
        //     bLCriticaItem.takt_time = bLCritical.takt_time;
        //     bLCriticaItem.update_by = bLCritical.update_by;
        //     bLCriticaItem.create_time = DateTime.Now;
        //     bLCriticaItem.create_by = bLCritical.create_by;

        //     //set file path & file name
        //     var filePath = bLCriticaItem.factory_id + "/" + "BestLine" + "/" + bLCriticaItem.line_id + "/" + bLCriticaItem.line_type_id + "/" + bLCriticaItem.model_no + "/" + "CriticalProcess";
        //     //best practice
        //     if (bLCritical.best_practice_url == null || bLCritical.best_practice_url == "")
        //     {
        //         bLCriticaItem.best_practice_url = bLCriticaItem.factory_id + "/no-image.jpg";
        //     }
        //     else
        //     {
        //         bLCriticaItem.best_practice_url = filePath + "/" + await fn.UploadAsync(bLCritical.best_practice_url, "uploaded/" + filePath, "BestPractice_" + Guid.NewGuid().ToString());
        //     }

        //     //man media
        //     if (bLCritical.man_media_url == null || bLCritical.man_media_url == "")
        //     {
        //         bLCriticaItem.man_media_url = bLCriticaItem.factory_id + "/no-image.jpg";
        //     }
        //     else
        //     {
        //         bLCriticaItem.man_media_url = filePath + "/" + await fn.UploadAsync(bLCritical.man_media_url, "uploaded" + "/" + filePath, "Man_" + Guid.NewGuid().ToString());
        //     }

        //     //method media
        //     if (bLCritical.method_media_url == null || bLCritical.method_media_url == "")
        //     {
        //         bLCriticaItem.method_media_url = bLCriticaItem.factory_id + "/no-image.jpg";
        //     }
        //     else
        //     {
        //         bLCriticaItem.method_media_url = filePath + "/" + await fn.UploadAsync(bLCritical.method_media_url, "uploaded/" + filePath, "Method_" + Guid.NewGuid().ToString());
        //     }

        //     //material media
        //     if (bLCritical.material_media_url == null || bLCritical.material_media_url == "")
        //     {
        //         bLCriticaItem.material_media_url = bLCriticaItem.factory_id + "/no-image.jpg";
        //     }
        //     else
        //     {
        //         bLCriticaItem.material_media_url = filePath + "/" + await fn.UploadAsync(bLCritical.material_media_url, "uploaded/" + filePath, "Material_" + Guid.NewGuid().ToString());
        //     }

        //     //machine media
        //     if (bLCritical.machine_media_url == null || bLCritical.machine_media_url == "")
        //     {
        //         bLCriticaItem.machine_media_url = bLCriticaItem.factory_id + "/no-image.jpg";
        //     }
        //     else
        //     {
        //         bLCriticaItem.machine_media_url = filePath + "/" + await fn.UploadAsync(bLCritical.machine_media_url, "uploaded/" + filePath, "Machine_" + Guid.NewGuid().ToString());
        //     }

        //     _iBLCriticalProcessAnalysisRepo.Add(bLCriticaItem);
        //     try
        //     {
        //         await _iBLCriticalProcessAnalysisRepo.SaveAll();
        //         return new OperationResult { Success = true, Caption = "Add This Critical Process Analysis was Succeeded ." };
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return new OperationResult { Success = false, Caption = "Critical Process Analysis Member failed on save.", Message = ex.ToString() };
        //     }
        // }


        // public async Task<OperationResult> Update(BL_Critical_Process_AnalysisDTO bLCritical)
        // {
        //     if (await IsExists(bLCritical) == false)
        //     {
        //         return new OperationResult { Success = false, Caption = "failed" };
        //     }

        //     var fn = new FunctionUtility();
        //     var bLCriticaItem = new BL_Critical_Process_Analysis();
        //     var currentCriticalProcessAnalysis = await _iBLCriticalProcessAnalysisRepo
        //          .FindAll(x => x.model_no == bLCritical.model_no
        //              && x.line_id == bLCritical.line_id
        //              && x.line_type_id == bLCritical.line_type_id
        //              && x.operation_id == bLCritical.operation_id
        //              && x.stage_id == bLCritical.stage_id
        //              && x.factory_id == bLCritical.factory_id)
        //          .AsNoTracking().FirstOrDefaultAsync();

        //     bLCriticaItem.factory_id = _configuration.GetSection("Appsettings:Factory").Value;
        //     bLCriticaItem.stage_id = bLCritical.stage_id;
        //     bLCriticaItem.line_id = bLCritical.line_id;
        //     bLCriticaItem.operation_id = bLCritical.operation_id;
        //     bLCriticaItem.line_type_id = bLCritical.line_type_id;
        //     bLCriticaItem.model_no = bLCritical.model_no;
        //     bLCriticaItem.ct_after_sec = bLCritical.ct_after_sec;
        //     bLCriticaItem.ct_before_sec = bLCritical.ct_before_sec;
        //     bLCriticaItem.machine_remarks = bLCritical.machine_remarks;
        //     bLCriticaItem.man_remarks = bLCritical.man_remarks;
        //     bLCriticaItem.material_remarks = bLCritical.material_remarks;
        //     bLCriticaItem.method_remarks = bLCritical.method_remarks;
        //     bLCriticaItem.takt_time = bLCritical.takt_time;
        //     bLCriticaItem.update_by = bLCritical.update_by;
        //     bLCriticaItem.update_time = DateTime.Now;

        //     var fileCheck = bLCriticaItem.factory_id + "/no-image.jpg";
        //     var filePath = bLCriticaItem.factory_id + "/" + "BestLine" + "/" + bLCriticaItem.line_id + "/" + bLCriticaItem.line_type_id + "/" + bLCriticaItem.model_no + "/" + "CriticalProcess";

        //     if (!string.IsNullOrEmpty(bLCritical.best_practice_url))
        //     {
        //         if (!string.IsNullOrEmpty(currentCriticalProcessAnalysis.best_practice_url) && currentCriticalProcessAnalysis.best_practice_url != fileCheck)
        //         {
        //             var currentbestPracticeMediaPath = Path.Combine(_webhostEnvironment.WebRootPath, "uploaded/", currentCriticalProcessAnalysis.best_practice_url);
        //             fn.DeleteFile(currentbestPracticeMediaPath);
        //         }
        //         var bestPracticeMedia = await fn.UploadAsync(bLCritical.best_practice_url, "uploaded/" + filePath, "BestPractice_" + Guid.NewGuid().ToString());
        //         bLCriticaItem.best_practice_url = filePath + "/" + bestPracticeMedia;
        //     }
        //     else
        //     {
        //         bLCriticaItem.best_practice_url = currentCriticalProcessAnalysis.best_practice_url;
        //     }

        //     //machine-media
        //     if (!string.IsNullOrEmpty(bLCritical.machine_media_url))
        //     {
        //         if (!string.IsNullOrEmpty(currentCriticalProcessAnalysis.machine_media_url) && currentCriticalProcessAnalysis.machine_media_url != fileCheck)
        //         {
        //             var currentMachineMediaPath = Path.Combine(_webhostEnvironment.WebRootPath, "uploaded/", currentCriticalProcessAnalysis.machine_media_url);
        //             fn.DeleteFile(currentMachineMediaPath);
        //         }
        //         var machineMedia = await fn.UploadAsync(bLCritical.machine_media_url, "uploaded/" + filePath, "Machine_" + Guid.NewGuid().ToString());
        //         bLCriticaItem.machine_media_url = filePath + "/" + machineMedia;
        //     }
        //     else
        //     {
        //         bLCriticaItem.machine_media_url = currentCriticalProcessAnalysis.machine_media_url;
        //     }

        //     //man-media
        //     if (!string.IsNullOrEmpty(bLCritical.man_media_url))
        //     {
        //         if (!string.IsNullOrEmpty(currentCriticalProcessAnalysis.man_media_url) && currentCriticalProcessAnalysis.man_media_url != fileCheck)
        //         {
        //             var currentManMediaPath = Path.Combine(_webhostEnvironment.WebRootPath, "uploaded/", currentCriticalProcessAnalysis.man_media_url);
        //             fn.DeleteFile(currentManMediaPath);
        //         }
        //         var manMedia = await fn.UploadAsync(bLCritical.man_media_url, "uploaded/" + filePath, "Man_" + Guid.NewGuid().ToString());
        //         bLCriticaItem.man_media_url = filePath + "/" + manMedia;
        //     }
        //     else
        //     {
        //         bLCriticaItem.man_media_url = currentCriticalProcessAnalysis.man_media_url;
        //     }

        //     //method-media
        //     if (!string.IsNullOrEmpty(bLCritical.method_media_url))
        //     {
        //         if (!string.IsNullOrEmpty(currentCriticalProcessAnalysis.method_media_url) && currentCriticalProcessAnalysis.method_media_url != fileCheck)
        //         {
        //             var currentMethodMediaPath = Path.Combine(_webhostEnvironment.WebRootPath, "uploaded/", currentCriticalProcessAnalysis.method_media_url);
        //             fn.DeleteFile(currentMethodMediaPath);
        //         }
        //         var methodMedia = await fn.UploadAsync(bLCritical.method_media_url, "uploaded/" + filePath, "Method_" + Guid.NewGuid().ToString());
        //         bLCriticaItem.method_media_url = filePath + "/" + methodMedia;
        //     }
        //     else
        //     {
        //         bLCriticaItem.method_media_url = currentCriticalProcessAnalysis.method_media_url;
        //     }

        //     //material-media
        //     if (!string.IsNullOrEmpty(bLCritical.material_media_url))
        //     {
        //         if (!string.IsNullOrEmpty(currentCriticalProcessAnalysis.material_media_url) && currentCriticalProcessAnalysis.material_media_url != fileCheck)
        //         {
        //             var currentMaterialMediaPath = Path.Combine(_webhostEnvironment.WebRootPath, "uploaded/", currentCriticalProcessAnalysis.material_media_url);
        //             fn.DeleteFile(currentMaterialMediaPath);
        //         }
        //         var materialMedia = await fn.UploadAsync(bLCritical.material_media_url, "uploaded/" + filePath, "Material_" + Guid.NewGuid().ToString());
        //         bLCriticaItem.material_media_url = filePath + "/" + materialMedia;
        //     }
        //     else
        //     {
        //         bLCriticaItem.material_media_url = currentCriticalProcessAnalysis.material_media_url;
        //     }

        //     var creatdata = await _iBLCriticalProcessAnalysisRepo
        //          .FindAll(x => x.model_no == bLCritical.model_no
        //              && x.line_id == bLCritical.line_id
        //              && x.line_type_id == bLCritical.line_type_id
        //              && x.operation_id == bLCritical.operation_id
        //              && x.stage_id == bLCritical.stage_id
        //              && x.factory_id == bLCritical.factory_id)
        //              .Select(x => x.create_by)
        //          .FirstOrDefaultAsync();

        //     var updateData = await _iBLCriticalProcessAnalysisRepo
        //          .FindAll(x => x.model_no == bLCritical.model_no
        //              && x.line_id == bLCritical.line_id
        //              && x.line_type_id == bLCritical.line_type_id
        //              && x.operation_id == bLCritical.operation_id
        //              && x.stage_id == bLCritical.stage_id
        //              && x.factory_id == bLCritical.factory_id)
        //              .Select(x => x.create_time)
        //          .FirstOrDefaultAsync();

        //     bLCriticaItem.create_time = updateData;
        //     bLCriticaItem.create_by = creatdata;

        //     _iBLCriticalProcessAnalysisRepo.Update(bLCriticaItem);
        //     try
        //     {
        //         await _iBLCriticalProcessAnalysisRepo.SaveAll();
        //         return new OperationResult { Success = true, Caption = "Critical Process Analysis was successfully Update." };
        //     }
        //     catch (System.Exception ex)
        //     {
        //         return new OperationResult { Success = false, Caption = "Critical Process Analysis Member failed on save.", Message = ex.ToString() };
        //     }
        // }

        // public async Task<List<BL_LinesDTO>> GetLineNo()
        // {
        //     var bLLayoutQuery = _bLLayoutDesignOverallRepo.FindAll().Distinct();
        //     var bLLineQuery = _bLLinesRepo.FindAll().Distinct();
        //     var lineNo = await bLLayoutQuery
        //     .GroupJoin(bLLineQuery, x => new { x.factory_id, x.line_id }, y => new { y.factory_id, y.line_id },
        //         (x, y) => new { a = x, b = y })
        //     .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new BL_LinesDTO
        //     {
        //         line_id = a.a.line_id,
        //         line_name = b.line_name,
        //     }).Distinct().ToListAsync();
        //     return lineNo;
        // }

        // public async Task<List<BL_Line_TypeDTO>> GetLineType(string line_id)
        // {
        //     var bLLayoutQuery = _bLLayoutDesignOverallRepo.FindAll(x => x.line_id == line_id).Distinct();
        //     var bLLineQuery = _bLLineTypeRepo.FindAll().Distinct();
        //     var lineType = await bLLayoutQuery
        //     .GroupJoin(bLLineQuery, x => new { x.factory_id, x.line_type_id }, y => new { y.factory_id, y.line_type_id },
        //         (x, y) => new { a = x, b = y })
        //     .SelectMany(x => x.b.DefaultIfEmpty(), (a, b) => new BL_Line_TypeDTO
        //     {
        //         line_type_id = a.a.line_type_id,
        //         line_type_name = b.line_type_name,
        //     }).Distinct().ToListAsync();
        //     return lineType;
        // }

        // public async Task<List<StageDTO>> GetStage()
        // {
        //     var data = await _stageRepo.FindAll(x => x.is_active == true).Select(x => new StageDTO
        //     {
        //         stage_id = x.stage_id,
        //         stage_name = x.stage_name,
        //         sequence = x.sequence
        //     }).OrderBy(x => x.sequence).ToListAsync();
        //     return data;
        // }

        // public async Task<List<ModelDTO>> GetModelNo()
        // {
        //     var data = await _modelRepo.FindAll(x => x.is_active == true).Select(x => new ModelDTO
        //     {
        //         model_no = x.model_no,
        //         model_name = x.model_name
        //     }).ToListAsync();
        //     return data;
        // }

        // public async Task<List<ModelOperationDTO>> GetOperationName(string model_no, string stage)
        // {
        //     var data = await _modelOperationRepo.FindAll(x => x.model_no == model_no && x.stage_id == stage).Select(x => new ModelOperationDTO
        //     {
        //         operation_id = x.operation_id,
        //         operation_name_en = x.operation_name_en,
        //         operation_name_local = x.operation_name_local,
        //         operation_name_zh = x.operation_name_zh
        //     }).ToListAsync();
        //     return data;
        // }

        // public async Task<BL_Critical_Process_AnalysisDTO> GetData(CriticalParam param)
        // {
        //     var data = await _iBLCriticalProcessAnalysisRepo
        //        .FindAll(x => x.line_id == param.line_id.Trim()
        //            && x.line_type_id == param.line_type_id.Trim()
        //            && x.model_no == param.model_no.Trim()
        //            && x.stage_id == param.stage_id.Trim()
        //            && x.operation_id == param.operation_id.Trim())
        //        .ProjectTo<BL_Critical_Process_AnalysisDTO>(_configMapper).FirstOrDefaultAsync();
        //     return data;}

    }
}
