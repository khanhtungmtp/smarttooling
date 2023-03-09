using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Models.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class C2BLayoutByProcessService : IC2BLayoutByProcessService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IModelRepository _modelRepository;
        private readonly IBLLinesRepository _linesRepository;
        private readonly IBLLayoutDesignProcessRepository _layoutDesignProcessRepository;
        private readonly IBLLineTypeRepository _lineTypeRepository;
        private readonly IBLLayoutDesignOverallRepository _layoutDesignOverallRepository;
        private readonly IProcessTypeRepository _processTypeRepository;

        public C2BLayoutByProcessService(
            IMapper mapper,
            MapperConfiguration configMapper,
            IModelRepository modelRepository,
            IBLLinesRepository linesRepository,
            IBLLayoutDesignProcessRepository layoutDesignProcessRepository,
            IBLLineTypeRepository lineTypeRepository,
            IBLLayoutDesignOverallRepository layoutDesignOverallRepository,
            IProcessTypeRepository processTypeRepository
        )
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _modelRepository = modelRepository;
            _linesRepository = linesRepository;
            _layoutDesignProcessRepository = layoutDesignProcessRepository;
            _lineTypeRepository = lineTypeRepository;
            _layoutDesignOverallRepository = layoutDesignOverallRepository;
            _processTypeRepository = processTypeRepository;
        }


        // public async Task<object> GetLineID()
        // {
        //     var blLayout = _layoutDesignOverallRepository.FindAll();
        //     var blLine = _linesRepository.FindAll();
        //     var data = await (from a in blLayout
        //                       join b in blLine on new { factory = a.factory_id, lineID = a.line_id }
        //                       equals new { factory = b.factory_id, lineID = b.line_id }
        //                       select new BL_LinesDTO()
        //                       {
        //                           line_id = a.line_id,
        //                           line_name = b.line_name,
        //                           sequence = b.sequence
        //                       }).Distinct().OrderBy(x => x.sequence).ToListAsync();
        //     return data.Select(x => new { id = x.line_id, name = x.line_name }).ToList();
        // }

        // public async Task<object> GetLineType(string lineID)
        // {
        //     var blLayoutPred = PredicateBuilder.New<BL_Layout_Design_Overall>(true);
        //     if (!string.IsNullOrEmpty(lineID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_id == lineID);
        //     }
        //     var blLayout = _layoutDesignOverallRepository.FindAll(blLayoutPred);
        //     var blLineType = _lineTypeRepository.FindAll();
        //     var data = await (from a in blLayout
        //                       join b in blLineType on new { factory = a.factory_id, lineTypeID = a.line_type_id }
        //                       equals new { factory = b.factory_id, lineTypeID = b.line_type_id }
        //                       select new BL_Line_TypeDTO()
        //                       {
        //                           line_type_id = a.line_type_id,
        //                           line_type_name = b.line_type_name,
        //                           sequence = b.sequence
        //                       }).Distinct().OrderBy(x => x.sequence).ToListAsync();
        //     return data.Select(x => new { lineTypeID = x.line_type_id, lineTypeName = x.line_type_name }).ToList();
        // }

        // public async Task<object> GetModelNo(string lineID, string lineTypeID)
        // {
        //     var blLayoutPred = PredicateBuilder.New<BL_Layout_Design_Overall>(true);
        //     if (!string.IsNullOrEmpty(lineID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_id == lineID);
        //     }
        //     if (!string.IsNullOrEmpty(lineTypeID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_type_id == lineTypeID);
        //     }
        //     var blLayout = _layoutDesignOverallRepository.FindAll(blLayoutPred);
        //     var model = _modelRepository.FindAll();
        //     var data = await blLayout.GroupJoin(model, x => new { x.factory_id, x.model_no },
        //     y => new { y.factory_id, y.model_no }, (x, y) => new { layout = x, modelno = y })
        //     .SelectMany(x => x.modelno.DefaultIfEmpty(), (x, y) => new
        //     {
        //         id = x.layout.model_no,
        //         text = y.model_name,
        //     }).Distinct().OrderBy(x => x.id).ToListAsync();
        //     return data;
        // }

        // public async Task<PagedList<BL_Layout_Design_Process_Params_DTO>> Search(string lineID, string lineTypeID, string modelNo, PaginationParams param)
        // {
        //     var blLayoutPred = PredicateBuilder.New<BL_Layout_Design_Process>(true);
        //     if (!string.IsNullOrEmpty(lineID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_id == lineID);
        //     }
        //     if (!string.IsNullOrEmpty(lineTypeID))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.line_type_id == lineTypeID);
        //     }
        //     if (!string.IsNullOrEmpty(modelNo))
        //     {
        //         blLayoutPred = blLayoutPred.And(x => x.model_no == modelNo);
        //     }
        //     var blLayout = _layoutDesignProcessRepository.FindAll(blLayoutPred);

        //     var blLine = _linesRepository.FindAll();
        //     var blLineType = _lineTypeRepository.FindAll();
        //     var model = _modelRepository.FindAll();
        //     var process = _processTypeRepository.FindAll();
        //     var data = (from a in blLayout
        //                 join b in blLine on new { factory = a.factory_id, lineID = a.line_id }
        //                 equals new { factory = b.factory_id, lineID = b.line_id }
        //                 join c in blLineType on new { factory = a.factory_id, lineTypeID = a.line_type_id }
        //                 equals new { factory = c.factory_id, lineTypeID = c.line_type_id }
        //                 join d in model on new { factory = a.factory_id, modelNo = a.model_no }
        //                 equals new { factory = d.factory_id, modelNo = d.model_no }
        //                 join e in process on new { factory = a.factory_id, processTypeID = a.process_type_id }
        //                 equals new { factory = e.factory_id, processTypeID = e.process_type_id }
        //                 select new BL_Layout_Design_Process_Params_DTO()
        //                 {
        //                     line_id = a.line_id,
        //                     line_name = b.line_name,
        //                     line_type_name = c.line_type_name,
        //                     model_no = a.model_no,
        //                     model_name = d.model_name,
        //                     each_process_image_after = a.each_process_image_after,
        //                     each_process_image_before = a.each_process_image_before,
        //                     remarks = a.remarks,
        //                     line_type_id = a.line_type_id,
        //                     process_type_id = e.process_type_id,
        //                     process_type_name_en = e.process_type_name_en,
        //                     update_by = a.update_by,
        //                     update_time = a.update_time
        //                 });
        //     return await PagedList<BL_Layout_Design_Process_Params_DTO>.CreateAsync(data, param.PageNumber, param.PageSize);
        // }
        // public async Task<bool> IsExists(BL_Layout_Design_ProcessDTO lineID)
        // {
        //     var lineDTO = await _layoutDesignProcessRepository
        //         .FindAll(x => x.line_id == lineID.line_id
        //                    && x.line_type_id == lineID.line_type_id
        //                    && x.model_no == lineID.model_no
        //                    && x.process_type_id == lineID.process_type_id)
        //         .AsNoTracking().FirstOrDefaultAsync();
        //     return lineDTO == null ? false : true;
        // }
        // public async Task<OperationResult> AddLayoutByProcess(BL_Layout_Design_ProcessDTO lineID)
        // {
        //     if (await IsExists(lineID))
        //         return new OperationResult { Success = false, Caption = "BL Layout Design Process already exists." };
        //     else
        //     {
        //         var item = _mapper.Map<BL_Layout_Design_Process>(lineID);
        //         _layoutDesignProcessRepository.Add(item);
        //         try
        //         {
        //             await _layoutDesignProcessRepository.SaveAll();
        //             return new OperationResult { Success = true, Caption = "BL Layout Design Process was successfully added." };
        //         }
        //         catch (System.Exception ex)
        //         {
        //             return new OperationResult { Success = false, Caption = "Adding BL Layout Design Process failed on save.", Message = ex.ToString() };
        //         }
        //     }
        // }

        // public async Task<OperationResult> EditLayoutByProcess(BL_Layout_Design_ProcessDTO lineID)
        // {
        //     if (!await IsExists(lineID))
        //         return new OperationResult { Success = false, Caption = "BL Layout Design Process not found." };
        //     else
        //     {
        //         lineID.line_id = lineID.line_id.Trim();
        //         var item = _mapper.Map<BL_Layout_Design_Process>(lineID);
        //         _layoutDesignProcessRepository.Update(item);
        //         try
        //         {
        //             await _layoutDesignProcessRepository.SaveAll();
        //             return new OperationResult { Success = true, Caption = "BL Layout Design Process was successfully updated." };
        //         }
        //         catch (System.Exception ex)
        //         {
        //             return new OperationResult { Success = false, Caption = "Updating BL Layout Design Process failed on save.", Message = ex.ToString() };
        //         }
        //     }
        // }

        // public async Task<object> GetProcessType()
        // {
        //     return await _processTypeRepository.FindAll(x => x.is_active == true)
        //     .OrderBy(x => x.sequence)
        //     .Select(x => new { Id = x.process_type_id, Text = x.process_type_name_en })
        //     .ToListAsync();
        // }

        // public async Task<BL_Layout_Design_ProcessDTO> GetDataEdit(string factory, string lineID, string lineTypeID, string modelNo, string processNo)
        // {
        //     return await _layoutDesignProcessRepository.FindAll(x => x.factory_id == factory &&
        //                                                              x.line_id == lineID &&
        //                                                              x.line_type_id == lineTypeID &&
        //                                                              x.model_no == modelNo &&
        //                                                              x.process_type_id == processNo)
        //                                                 .ProjectTo<BL_Layout_Design_ProcessDTO>(_configMapper)
        //                                                 .FirstOrDefaultAsync();
        // }

    }
}