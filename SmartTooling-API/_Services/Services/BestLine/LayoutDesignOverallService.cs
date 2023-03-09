using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.BestLine;
using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.DTO.BestLine.LayoutDesignOverall;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;
using SmartTooling_API.Models.BestLine;

namespace SmartTooling_API._Services.Services.BestLine
{
    public class LayoutDesignOverallService : ILayoutDesignOverallService
    {
        private readonly IBLLayoutDesignOverallRepository _repoLayoutDesignOverall;
        private readonly IBLLinesRepository _repoLines;
        private readonly IBLLineTypeRepository _repoLineType;
        private readonly IModelRepository _repoModel;
        private readonly IMapper _mapper;
        public LayoutDesignOverallService(IBLLayoutDesignOverallRepository repoLayoutDesignOverall, IMapper mapper, IBLLineTypeRepository repoLineType, IModelRepository repoModel, IBLLinesRepository repoLines)
        {
            _repoLayoutDesignOverall = repoLayoutDesignOverall;
            _mapper = mapper;
            _repoLineType = repoLineType;
            _repoModel = repoModel;
            _repoLines = repoLines;
        }
        public async Task<OperationResult> AddLayoutDesignOverall(BL_Layout_Design_OverallDTO model)
        {
            if (await IsExists(model))
            {
                return new OperationResult { Success = false, Message = "BL Layout Design Overall already exists." };
            }
            else
            {
                var models = _mapper.Map<BL_Layout_Design_Overall>(model);
                _repoLayoutDesignOverall.Add(models);
                try
                {
                    await _repoLayoutDesignOverall.SaveAll();
                    return new OperationResult { Success = true, Message = "BL Layout Design Overall was successfully added." };
                }
                catch (System.Exception ex)
                {
                    return new OperationResult { Success = false, Message = "Adding BL Layout Design Overall failed on save.", Caption = ex.ToString() };
                }
            }
        }
        public async Task<object> GetLineNoFromBL_Layout_Design_Overall() => await _repoLayoutDesignOverall.FindAll().GroupJoin(_repoLines.FindAll(x => x.is_active == true),
            x => x.line_id,
            y => y.line_id,
            (x, y) => new { T1 = x, T2 = y }).SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = y }).Select(x => new
            {
                line_id = x.T1.line_id,
                line_name = x.T2.line_name,
                sequence = x.T2.sequence
            }).Distinct().OrderBy(x => x.sequence).ToListAsync();

        public async Task<object> GetLineTypeBL_Layout_Design_Overall()
        => await _repoLayoutDesignOverall
                .FindAll().GroupJoin(_repoLineType.FindAll(x => x.is_active == true),
                x => x.line_type_id,
                y => y.line_type_id,
                (x, y) => new { T1 = x, T2 = y }).SelectMany(x => x.T2.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = y })
                .OrderBy(x => x.T2.line_type_id)
                .Select(x => new
                {
                    line_type_id = x.T1.line_type_id.Trim(),
                    line_type_name = x.T2.line_type_name.Trim(),
                })
                .Distinct()
                .ToListAsync();

        public async Task<object> GetAllProdSeason() => await _repoLayoutDesignOverall.FindAll().OrderBy(x => x.prod_season).Select(x => new
        {
            prod_season = x.prod_season.Trim()
        }).Distinct().ToListAsync();

        public async Task<object> GetAllModelNo()
        => await _repoModel
        .FindAll(x => x.is_active == true && x.pilot_line == true)
        .Select(x => new
        {
            model_no = x.model_no.Trim(),
            model_name = x.model_name.Trim()
        }).Distinct().ToListAsync();

        public async Task<object> GetAllLineNo()
        => await _repoLines
        .FindAll(x => x.is_active == true)
        .OrderBy(x => x.sequence)
        .Select(x => new
        {
            line_id = x.line_id.Trim(),
            line_name = x.line_name.Trim()
        }).Distinct().ToListAsync();

        public async Task<object> GetAllLineType()
        => await _repoLineType
                .FindAll(x => x.is_active == true)
                .OrderBy(x => x.sequence)
                .Select(x => new
                {
                    line_type_id = x.line_type_id.Trim(),
                    line_type_name = x.line_type_name.Trim(),
                })
                .Distinct()
                .ToListAsync();

        public async Task<bool> UpdateLayoutDesignOverall(BL_Layout_Design_OverallDTO model)
        {
            var result = await _repoLayoutDesignOverall.FindAll(x => x.factory_id == model.factory_id && x.line_id == model.line_id && x.line_type_id == model.line_type_id && x.model_no == model.model_no).FirstOrDefaultAsync();
            result.prod_season = model.prod_season;
            result.no_of_process_before = model.no_of_process_before;
            result.no_of_process_after = model.no_of_process_after;
            result.tct_before = model.tct_before;
            result.tct_after = model.tct_after;
            result.cps_mp_before = model.cps_mp_before;
            result.cps_mp_after = model.cps_mp_after;
            result.assembly_mp_before = model.assembly_mp_before;
            result.assembly_mp_after = model.assembly_mp_after;
            result.eolr_before = model.eolr_before;
            result.eolr_after = model.eolr_after;
            result.ller_before_percent = model.ller_before_percent;
            result.ller_after_percent = model.ller_after_percent;
            result.tentative_pph_before = model.tentative_pph_before;
            result.tentative_pph_after = model.tentative_pph_after;
            result.tentative_efficiency_before_percent = model.tentative_efficiency_before_percent;
            result.tentative_efficiency_after_percent = model.tentative_efficiency_after_percent;
            result.c2b_overall_image = model.c2b_overall_image;
            result.update_by = model.update_by;
            result.update_time = model.update_time;
            try
            {
                _repoLayoutDesignOverall.Update(result);
                await _repoLayoutDesignOverall.SaveAll();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<PagedList<LayoutDesignOverallModelDTO>> Search(PaginationParams pagination, LayoutDesignOverallParam filterParam)
        {
            var layoutPred = PredicateBuilder.New<BL_Layout_Design_Overall>(true);
            if (!string.IsNullOrEmpty(filterParam.line_no))
                layoutPred.And(x => x.line_id.Trim() == filterParam.line_no.Trim());
            if (!string.IsNullOrEmpty(filterParam.line_type))
                layoutPred.And(x => x.line_type_id.Trim() == filterParam.line_type.Trim());
            if (!string.IsNullOrEmpty(filterParam.prod_season))
                layoutPred.And(x => x.prod_season.Trim() == filterParam.prod_season.Trim());

            var layoutDesignOverall = _repoLayoutDesignOverall.FindAll(layoutPred);
            var model = _repoModel.FindAll();
            var lines = _repoLines.FindAll();
            var lineType = _repoLineType.FindAll();

            var resultList = layoutDesignOverall.GroupJoin(lines,
              x => new { x.factory_id, x.line_id },
                y => new { y.factory_id, y.line_id },
            (x, y) => new { T1 = x, T2 = y }).SelectMany(
                    x => x.T2.DefaultIfEmpty(),
                    (x, y) => new { T1 = x.T1, T2 = y })
                    .GroupJoin(
                        lineType,
                        x => new { x.T1.factory_id, x.T1.line_type_id },
                        y => new { y.factory_id, y.line_type_id },
                        (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
                        .SelectMany(
                        x => x.T3.DefaultIfEmpty(),
                        (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = y })
                        .GroupJoin(
                            model,
                            x => new { x.T1.factory_id, x.T1.model_no },
                            y => new { y.factory_id, y.model_no },
                            (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = y })
                            .SelectMany(x => x.T4.DefaultIfEmpty(), (x, y) => new { T1 = x.T1, T2 = x.T2, T3 = x.T3, T4 = y })
                            .OrderBy(x => x.T1.line_id)
                    .ThenBy(x => x.T1.model_no)
                    .ThenBy(x => x.T1.prod_season)
                            .Select(x => new LayoutDesignOverallModelDTO
                            {
                                factory_id = x.T1.factory_id,
                                line_id = x.T1.line_id,
                                line_name = x.T2.line_name,
                                line_type_id = x.T1.line_type_id,
                                line_type_name = x.T3.line_type_name,
                                model_no = x.T1.model_no.ToUpper(),
                                model_name = x.T4.model_name,
                                prod_season = x.T1.prod_season,
                                no_of_process_before = x.T1.no_of_process_before,
                                no_of_process_after = x.T1.no_of_process_after,
                                tct_before = x.T1.tct_before,
                                tct_after = x.T1.tct_after,
                                cps_mp_before = x.T1.cps_mp_before,
                                cps_mp_after = x.T1.cps_mp_after,
                                assembly_mp_before = x.T1.assembly_mp_before,
                                assembly_mp_after = x.T1.assembly_mp_after,
                                eolr_before = x.T1.eolr_before,
                                eolr_after = x.T1.eolr_after,
                                ller_before_percent = x.T1.ller_before_percent,
                                ller_after_percent = x.T1.ller_after_percent,
                                tentative_pph_before = x.T1.tentative_pph_before,
                                tentative_pph_after = x.T1.tentative_pph_after,
                                tentative_efficiency_before_percent = x.T1.tentative_efficiency_before_percent,
                                tentative_efficiency_after_percent = x.T1.tentative_efficiency_after_percent,
                                c2b_overall_image = x.T1.c2b_overall_image,
                                update_by = x.T1.update_by,
                                update_time = x.T1.update_time
                            });

            if (!string.IsNullOrEmpty(filterParam.model))
            {
                resultList = resultList.Where(x => x.model_no.Contains(filterParam.model) || x.model_name.Contains(filterParam.model));
            }
            return await PagedList<LayoutDesignOverallModelDTO>.CreateAsync(resultList, pagination.PageNumber, pagination.PageSize);
        }
        public async Task<bool> IsExists(BL_Layout_Design_OverallDTO model)
        {
            var item = await _repoLayoutDesignOverall.FindAll(x => x.factory_id == model.factory_id &&
                                                  x.line_id == model.line_id &&
                                                  x.line_type_id == model.line_type_id &&
                                                  x.model_no == model.model_no).AsNoTracking().FirstOrDefaultAsync();
            return item == null ? false : true;
        }

        public async Task<BL_Layout_Design_OverallDTO> GetParamsEdit(string factory_id, string line_id, string line_type_id, string model_no)
        {
            var model = await _repoLayoutDesignOverall.FindAll(x => x.factory_id == factory_id
                                                                 && x.line_id == line_id
                                                                 && x.line_type_id == line_type_id
                                                                 && x.model_no == model_no).FirstOrDefaultAsync();
            var mapEdit = _mapper.Map<BL_Layout_Design_OverallDTO>(model);
            return mapEdit;
        }
    }
}