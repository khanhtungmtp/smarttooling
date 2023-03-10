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

        public C2BLayoutAttachmentService(IMapper mapper, IBLAttachmentsRepository repoBLAttachment, IBLLinesRepository repoBLLines, IBLLineTypeRepository repoBLLineType, IBLAttachmentTypeRepository repoBLAttachmentType, IModelRepository repoModel, IBLLayoutDesignOverallRepository repoBLLayoutDesignOverall)
        {
            _mapper = mapper;
            _repoBLAttachment = repoBLAttachment;
            _repoBLLines = repoBLLines;
            _repoBLLineType = repoBLLineType;
            _repoBLAttachmentType = repoBLAttachmentType;
            _repoModel = repoModel;
            _repoBLLayoutDesignOverall = repoBLLayoutDesignOverall;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllAttachmentType() => await _repoBLAttachmentType.FindAll(x => x.is_active == true)
        .OrderBy(x => x.sequence)
        .Select(x => new KeyValuePair<string, string>(x.attachment_type_id.Trim(), x.attachment_type_name.Trim()))
        .Distinct().ToListAsync();


        public async Task<object> GetAllLineNo()
        {
            var layoutDesignOverall = _repoBLLayoutDesignOverall.FindAll().AsNoTracking()
            .GroupJoin(_repoBLAttachment.FindAll().AsNoTracking(),
            x => x.id,
            y => y.layout_design_overall_id,
            (x, y) => new { T1 = x, T2 = y })
            .SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = y })
            .GroupJoin(_repoBLLines.FindAll().AsNoTracking(),
            x => new { x.T1.factory_id, x.T1.line_id },
            y => new { y.factory_id, y.line_id },
            (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
            .SelectMany(x => x.T3.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
            .Select(x => new
            {
                x.T1.line_id,
                x.T3.line_name,
                x.T3.sequence
            }).Distinct().OrderBy(x => x.sequence).ToListAsync();
            return await layoutDesignOverall;
        }

        public async Task<object> GetAllLineType()
        {
            var layoutDesignOverall = _repoBLLayoutDesignOverall.FindAll().AsNoTracking().GroupJoin(_repoBLAttachment.FindAll().AsNoTracking(),
            x => x.id,
            y => y.layout_design_overall_id,
            (x, y) => new { T1 = x, T2 = y })
            .SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = y })
            .GroupJoin(_repoBLLineType.FindAll().AsNoTracking(),
            x => x.T1.line_type_id,
            y => y.line_type_id,
            (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y }).SelectMany(x => x.T3.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
            .Select(x => new
            {
                line_type_id = x.T1.line_type_id,
                line_type_name = x.T3.line_type_name,
                sequence = x.T3.sequence
            }).Distinct().OrderBy(x => x.sequence).ToListAsync();
            return await layoutDesignOverall;
        }

        public async Task<object> GetAllProdSeason() => await _repoBLLayoutDesignOverall.FindAll().OrderBy(x => x.prod_season).Select(x => new
        {
            prod_season = x.prod_season.Trim()
        }).Distinct().ToListAsync();

        public async Task<PagedList<C2B_Layout_AttachmentDTO>> Search(PaginationParams pagination, C2BLayoutAttachmentParam filterParam)
        {
            var data = _repoBLAttachment.FindAll().AsNoTracking()
          .GroupJoin(_repoBLLayoutDesignOverall.FindAll().AsNoTracking(),
          x => x.layout_design_overall_id,
          y => y.id,
          (x, y) => new { T1 = x, T2 = y })
          .SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = y })
          .GroupJoin(_repoBLLines.FindAll().AsNoTracking(),
          x => new { x.T2.factory_id, x.T2.line_id },
          y => new { y.factory_id, y.line_id },
          (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
          .SelectMany(x => x.T3.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
          .GroupJoin(_repoBLLineType.FindAll().AsNoTracking(),
          x => new { x.T2.factory_id, x.T2.line_type_id },
          y => new { y.factory_id, y.line_type_id },
          (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = y })
          .SelectMany(x => x.T4.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = y })
          .GroupJoin(_repoModel.FindAll().AsNoTracking(),
          x => new { x.T2.factory_id, x.T2.model_no },
          y => new { y.factory_id, y.model_no },
          (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = x.T4, T5 = y })
          .SelectMany(x => x.T5.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = x.T4, T5 = y })
          .GroupJoin(_repoBLAttachmentType.FindAll().AsNoTracking(),
          x => new { x.T2.factory_id, x.T1.attachment_type_id },
          y => new { y.factory_id, y.attachment_type_id },
          (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = x.T4, T5 = x.T5, T6 = y }
          ).SelectMany(x => x.T6.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = x.T4, T5 = x.T5, T6 = y })
            .OrderBy(x => x.T2.line_id)
            .ThenBy(x => x.T4.sequence)
            .ThenBy(x => x.T2.model_no)
            .ThenBy(x => x.T2.prod_season)
          .Select(x => new C2B_Layout_AttachmentDTO
          {
              id = x.T1.id,
              line_name = x.T3.line_name,
              line_type_name = x.T4.line_type_name,
              model_no = x.T2.model_no,
              model_name = x.T5.model_name,
              prod_season = x.T2.prod_season,
              attachment_type_name = x.T6.attachment_type_name,
              attachment_name = x.T1.attachment_name,
              attachment_file_url = x.T1.attachment_file_url
          });
            var predicate = PredicateBuilder.New<C2B_Layout_AttachmentDTO>(true);
            if (!string.IsNullOrWhiteSpace(filterParam.line_name))
                predicate.And(x => x.line_name.Trim() == filterParam.line_name.Trim());
            if (!string.IsNullOrWhiteSpace(filterParam.line_type_name))
                predicate.And(x => x.line_type_name.Trim() == filterParam.line_type_name.Trim());
            if (!string.IsNullOrWhiteSpace(filterParam.prod_season))
                predicate.And(x => x.prod_season.Trim() == filterParam.prod_season.Trim());
            if (!string.IsNullOrEmpty(filterParam.model))
                predicate.And(x => x.model_no.Contains(filterParam.model) || x.model_name.Contains(filterParam.model));
            data = data.Where(predicate);
            return await PagedList<C2B_Layout_AttachmentDTO>.CreateAsync(data, pagination.PageNumber, pagination.PageSize, true);
        }

        public async Task<bool> Create(BL_AttachmentsDTO model)
        {
            if (await IsExists(model))
            {
                return false;
            }
            else
            {
                var models = _mapper.Map<BL_Attachments>(model);
                _repoBLAttachment.Add(models);
                var result = await _repoBLAttachment.SaveAll();
                return result;
            }
        }


        public async Task<bool> DeleteAttachment(BL_AttachmentsParams model)
        {
            var item = _mapper.Map<BL_Attachments>(model);
            _repoBLAttachment.Remove(item);
            try
            {
                return await _repoBLAttachment.SaveAll();
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<object> GetAllLineNoOfAdd() => await _repoBLLayoutDesignOverall.FindAll().GroupJoin(_repoBLLines.FindAll(x => x.is_active == true),
           x => new { x.line_id, x.factory_id },
           y => new { y.line_id, y.factory_id },
           (x, y) => new { T1 = x, T2 = y }).SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = y }).Select(x => new
           {
               line_id = x.T1.line_id,
               line_name = x.T2.line_name,
               sequence = x.T2.sequence
           }).Distinct().OrderBy(x => x.sequence).ToListAsync();


        public async Task<object> GetAllLineTypeOfAdd(string line_id) => await _repoBLLayoutDesignOverall
                .FindAll(x => x.line_id.Trim() == line_id.Trim()).GroupJoin(_repoBLLineType.FindAll(x => x.is_active == true),
                x => new { x.line_type_id, x.factory_id },
                y => new { y.line_type_id, y.factory_id },
                (x, y) => new { T1 = x, T2 = y }).SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = y })
                .Select(x => new
                {
                    line_type_id = x.T1.line_type_id.Trim(),
                    line_type_name = x.T2.line_type_name.Trim(),
                    sequence = x.T2.sequence
                })
                .Distinct().OrderBy(x => x.sequence)
                .ToListAsync();

        public async Task<object> GetAllModelNoOfAdd(string line_id, string line_type_id) => await _repoBLLayoutDesignOverall
        .FindAll(x => x.line_id.Trim() == line_id.Trim() && x.line_type_id.Trim() == line_type_id.Trim()).AsNoTracking().GroupJoin(_repoModel.FindAll().AsNoTracking(),
        x => new { x.factory_id, x.model_no },
        y => new { y.factory_id, y.model_no },
        (x, y) => new { T1 = x, T2 = y })
        .SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = y })
        .Select(x => new
        {
            model_no = x.T1.model_no,
            model_name = x.T2.model_name
        }).Distinct().OrderBy(x => x.model_no).ToListAsync();

        public async Task<object> GetAllProdSeasonOfAdd(string line_id, string line_type_id, string model_no) => await _repoBLLayoutDesignOverall.FindAll(x => x.line_id.Trim() == line_id.Trim()
        && x.line_type_id.Trim() == line_type_id.Trim()
        && x.model_no.Trim() == model_no.Trim()).AsNoTracking()
        .Select(x => new
        {
            prod_season = x.prod_season
        }).OrderBy(x => x.prod_season).ToListAsync();

        public async Task<bool> IsExists(BL_AttachmentsDTO model)
        {
            var item = await _repoBLAttachment.FindAll(x => x.attachment_type_id == model.attachment_type_id)
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync();
            return item == null ? false : true;
        }

        public long GetLayout_design_overall_id(BL_AttachmentsParams model) =>
            _repoBLLayoutDesignOverall.FindSingle(x => x.line_id.Trim() == model.line_id.Trim()
            && x.line_type_id.Trim() == model.line_type_id.Trim()
             && x.model_no.Trim() == model.model_no.Trim()
             && x.prod_season.Trim() == model.prod_season.Trim()).id;
    }
}