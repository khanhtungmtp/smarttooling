using System;
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
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Models.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class C2BLayoutAttachmentService : IC2BLayoutAttachmentService
    {
        private readonly IMapper _mapper;
        private readonly IBLLayoutDesignOverallRepository _repoBLLayoutDesignOverall;
        private readonly IBLAttachmentsRepository _repoBLAttachment;
        private readonly IBLLinesRepository _repoBLLines;
        private readonly IBLLineTypeRepository _repoBLLineType;
        private readonly IBLAttachmentTypeRepository _repoBLAttachmentType;
        private readonly IModelRepository _repoModel;


        // public C2BLayoutAttachmentService(IMapper mapper, IBLAttachmentsRepository repoBLAttachment, IBLLinesRepository repoBLLines, IBLLineTypeRepository repoBLLineType, IBLAttachmentTypeRepository repoBLAttachmentType, IModelRepository repoModel, IBLLayoutDesignOverallRepository repoBLLayoutDesignOverall)
        // {
        //     _mapper = mapper;
        //     _repoBLAttachment = repoBLAttachment;
        //     _repoBLLines = repoBLLines;
        //     _repoBLLineType = repoBLLineType;
        //     _repoBLAttachmentType = repoBLAttachmentType;
        //     _repoModel = repoModel;
        //     _repoBLLayoutDesignOverall = repoBLLayoutDesignOverall;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetAllAttachmentType()
        // {
        //     var dataResult = await _repoBLAttachmentType.FindAll(x => x.is_active == true).OrderBy(x => x.sequence).Select(x => new KeyValuePair<string, string>(x.attachment_type_id.Trim(), x.attachment_type_name.Trim())).Distinct().ToListAsync();
        //     return dataResult;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetAllLineNo()
        // {
        //     var layoutDesignOverall = _repoBLLayoutDesignOverall.FindAll();
        //     var line = _repoBLLines.FindAll();
        //     var dataResult = await layoutDesignOverall.GroupJoin(line, x => new { x.factory_id, x.line_id }, y => new { y.factory_id, y.line_id }, (x, y) => new { layoutDesignOverall = x, line = y }).SelectMany(x => x.line.DefaultIfEmpty(), (x, y) => new KeyValuePair<string, string>(x.layoutDesignOverall.line_id.Trim(), y.line_name.Trim())).Distinct().ToListAsync();
        //     return dataResult;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetAllLineType(string lineId)
        // {
        //     var layoutDesignOverall = _repoBLLayoutDesignOverall.FindAll(x => x.line_id == lineId);
        //     var lineType = _repoBLLineType.FindAll();
        //     var dataResult = await layoutDesignOverall.GroupJoin(lineType, x => new { x.factory_id, x.line_type_id }, y => new { y.factory_id, y.line_type_id }, (x, y) => new { layoutDesignOverall = x, lineType = y }).SelectMany(x => x.lineType.DefaultIfEmpty(), (x, y) => new KeyValuePair<string, string>(x.layoutDesignOverall.line_type_id.Trim(), y.line_type_name.Trim())).Distinct().ToListAsync();
        //     return dataResult;
        // }

        // public async Task<List<KeyValuePair<string, string>>> GetAllModelNo(string lineId, string lineTypeId)
        // {
        //     var layoutDesignOverall = _repoBLLayoutDesignOverall.FindAll(x => x.line_id == lineId && x.line_type_id == lineTypeId);
        //     var model = _repoModel.FindAll();
        //     var dataResult = await layoutDesignOverall.GroupJoin(model, x => new { x.factory_id, x.model_no }, y => new { y.factory_id, y.model_no }, (x, y) => new { layoutDesignOverall = x, model = y }).SelectMany(x => x.model.DefaultIfEmpty(), (x, y) => new KeyValuePair<string, string>(x.layoutDesignOverall.model_no.Trim(), y.model_name.Trim())).Distinct().ToListAsync();
        //     return dataResult;
        // }

        // public async Task<PagedList<C2B_Layout_AttachmentDTO>> Search(PaginationParams param, C2BLayoutAttachmentParam filterParam)
        // {
        //     var attachmentPred = PredicateBuilder.New<BL_Attachments>(true);
        //     if (!String.IsNullOrEmpty(filterParam.line_id))
        //     {
        //         attachmentPred = attachmentPred.And(x => x.line_id.Trim() == filterParam.line_id.Trim());
        //     }
        //     if (!String.IsNullOrEmpty(filterParam.line_type_id))
        //     {
        //         attachmentPred = attachmentPred.And(x => x.line_type_id.Trim() == filterParam.line_type_id.Trim());
        //     }
        //     if (!String.IsNullOrEmpty(filterParam.model))
        //     {
        //         attachmentPred = attachmentPred.And(x => x.model_no.Trim() == filterParam.model.Trim());
        //     }
        //     var attachments = _repoBLAttachment.FindAll(attachmentPred);
        //     var attachmentType = _repoBLAttachmentType.FindAll();
        //     var line = _repoBLLines.FindAll();
        //     var lineType = _repoBLLineType.FindAll();
        //     var model = _repoModel.FindAll();

        //     var dataResult = attachments.GroupJoin(
        //         line,
        //         x => new { x.factory_id, x.line_id },
        //         y => new { y.factory_id, y.line_id },
        //         (x, y) => new { attachments = x, line = y })
        //         .SelectMany(
        //             x => x.line.DefaultIfEmpty(),
        //             (x, y) => new { x.attachments, line = y })
        //             .GroupJoin(
        //                 lineType,
        //                 x => new { x.attachments.factory_id, x.attachments.line_type_id },
        //                 y => new { y.factory_id, y.line_type_id },
        //                 (x, y) => new { x.attachments, x.line, lineType = y })
        //                 .SelectMany(
        //                 x => x.lineType.DefaultIfEmpty(),
        //                 (x, y) => new { x.attachments, x.line, lineType = y })
        //                 .GroupJoin(
        //                     model,
        //                     x => new { x.attachments.factory_id, x.attachments.model_no },
        //                     y => new { y.factory_id, y.model_no },
        //                     (x, y) => new { x.attachments, x.line, x.lineType, model = y })
        //                     .SelectMany(
        //                         x => x.model.DefaultIfEmpty(),
        //                         (x, y) => new
        //                         {
        //                             x.attachments,
        //                             x.line,
        //                             x.lineType,
        //                             model = y
        //                         }).GroupJoin(
        //                             attachmentType,
        //                             x => new { x.attachments.factory_id, x.attachments.attachment_type_id },
        //                             y => new { y.factory_id, y.attachment_type_id },
        //                             (x, y) => new { x.attachments, x.line, x.lineType, x.model, attachmentType = y }).SelectMany(
        //                         x => x.attachmentType.DefaultIfEmpty(),
        //                         (x, y) => new C2B_Layout_AttachmentDTO
        //                         {
        //                             factory_id = x.attachments.factory_id,
        //                             line_id = x.attachments.line_id,
        //                             line_name = x.line.line_name,
        //                             line_type_id = x.attachments.line_type_id,
        //                             line_type_name = x.lineType.line_type_name,
        //                             model_no = x.attachments.model_no,
        //                             model_name = x.model.model_name,
        //                             attachment_type_name = y.attachment_type_name,
        //                             attachment_name = x.attachments.attachment_name,
        //                             attachment_file_url = x.attachments.attachment_file_url
        //                         });
        //     dataResult = dataResult.Where(x => x.line_id == filterParam.line_id && x.line_type_id == filterParam.line_type_id && x.model_no == filterParam.model);
        //     return await PagedList<C2B_Layout_AttachmentDTO>.CreateAsync(dataResult, param.PageNumber, param.PageSize);
        // }


        // public async Task<bool> Create(BL_AttachmentsDTO model)
        // {
        //     if (await IsExists(model))
        //     {
        //         return false;
        //     }
        //     else
        //     {
        //         var models = _mapper.Map<BL_Attachments>(model);
        //         _repoBLAttachment.Add(models);
        //         var result = await _repoBLAttachment.SaveAll();
        //         return result;
        //     }
        // }

        // public async Task<bool> DeleteAttachment(BL_AttachmentsDTO model)
        // {
        //     var item = _mapper.Map<BL_Attachments>(model);
        //     _repoBLAttachment.Remove(item);
        //     try
        //     {
        //         return await _repoBLAttachment.SaveAll();
        //     }
        //     catch (System.Exception)
        //     {
        //         return false;
        //     }
        // }


        // public async Task<bool> IsExists(BL_AttachmentsDTO model)
        // {
        //     var item = await _repoBLAttachment.FindAll(x => x.factory_id == model.factory_id &&
        //                                           x.line_id == model.line_id &&
        //                                           x.line_type_id == model.line_type_id &&
        //                                           x.model_no == model.model_no && x.attachment_name == model.attachment_name)
        //                                           .AsNoTracking()
        //                                           .FirstOrDefaultAsync();
        //     return item == null ? false : true;
        // }

    }
}