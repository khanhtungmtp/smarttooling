using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class KaizenReportService : IKaizenReportService
    {
        private readonly IModelRepository _repoModel;
        private readonly IViewModelKaizenRepository _repoViewModelKaizen;
        private readonly IEfficiencyRepository _repoEfficiency;
        private readonly IKaizenRepository _repoKaizen;
        private readonly IModelOperationRepository _repoModelOperation;
        private readonly IMapper _mapper;
        public KaizenReportService(IModelRepository repoModel,
                                    IMapper mapper,
                                    IEfficiencyRepository repoEfficiency,
                                    IKaizenRepository repoKaizen,
                                    IModelOperationRepository repoModelOperation,
                                    IViewModelKaizenRepository repoViewModelKaizen)
        {
            _repoModel = repoModel;
            _repoViewModelKaizen = repoViewModelKaizen;
            _repoEfficiency = repoEfficiency;
            _repoKaizen = repoKaizen;
            _repoModelOperation = repoModelOperation;
            _mapper = mapper;
        }

        public async Task<List<Efficiency>> GetEfficiencys(string factory_id, string upper_id, string season)
        {
            var data = await _repoEfficiency.FindAll(x => x.factory_id.Trim() == factory_id.Trim() && x.upper_id.Trim() == upper_id.Trim()
                    && x.season.Trim() == season.Trim()).OrderBy(x => x.sequence).ToListAsync();
            return data;
        }

        public async Task<object> GetKaizenDetail(string factory_id, string model_no, string serial_no)
        {
            var modelOperations = _repoModelOperation.FindAll();
            var kaizens = _repoKaizen.FindAll();
            var models = _repoModel.FindAll();
            var data = await (from a in kaizens
                              join b in models
                              on new { factoryId = a.factory_id.Trim(), modelNo = a.model_no.Trim() }
                              equals new { factoryId = b.factory_id.Trim(), modelNo = b.model_no.Trim() }
                              join c in modelOperations
                              on new { factoryId = a.factory_id.Trim(), modelNo = a.model_no.Trim(), operationId = a.operation_id.Trim(), stageId = a.stage_id.Trim() }
                              equals new { factoryId = c.factory_id.Trim(), modelNo = c.model_no.Trim(), operationId = c.operation_id.Trim(), stageId = c.stage_id.Trim() }
                              where a.factory_id.Trim() == factory_id.Trim() && a.model_no.Trim() == model_no.Trim()
                              && a.serial_no.ToString() == serial_no.Trim()
                              select new
                              {
                                  kaizen = a,
                                  model = b,
                                  modelOperation = c
                              }).FirstOrDefaultAsync();
            return data;
        }

        public async Task<PagedList<KaizenModelDetail>> GetKaiZens(PaginationParams param, string factory_id, string model_no)
        {
            var kaizens = _repoKaizen.FindAll(x => x.factory_id.Trim() == factory_id.Trim() && x.model_no.Trim() == model_no.Trim())
                    .OrderBy(x => x.serial_no);
            var modelOperations = _repoModelOperation.GetAll()
                    .Where(x => x.factory_id.Trim() == factory_id.Trim() &&
                    x.model_no.Trim() == model_no.Trim());
            var data = (from a in kaizens
                        join b in modelOperations
      on new { stage_id = a.stage_id.Trim(), operation_id = a.operation_id.Trim() }
      equals new { stage_id = b.stage_id.Trim(), operation_id = b.operation_id.Trim() }
                        select new KaizenModelDetail()
                        {
                            factory_id = a.factory_id,
                            model_no = a.model_no,
                            serial_no = a.serial_no,
                            kaizen_description = a.kaizen_description,
                            stage_id = a.stage_id,
                            process_type_id = b.process_type_id,
                            operation_id = b.operation_id,
                            start_date = a.start_date,
                            process_tct_sec = a.process_tct_sec,
                            ct_before_sec = a.ct_before_sec,
                            ct_after_sec = a.ct_after_sec,
                            improv = 0,
                            rft_before_percent = a.rft_before_percent,
                            rft_after_percent = a.rft_after_percent,
                            line_roll_out_percent = a.line_roll_out_percent,
                            clicks_times = a.clicks_times
                        }).OrderBy(x => x.serial_no);
            return await PagedList<KaizenModelDetail>.CreateAsync(data, param.PageNumber, param.PageSize);
        }

        public async Task<List<VW_ModelKaizen_Dto>> GetModelKaizens(string factory_id, KaizenReportParam filter)
        {
            var pred_modelKaizen = PredicateBuilder.New<VW_ModelKaizen>(true);
            pred_modelKaizen.And(x => x.factory_id.Trim() == factory_id.Trim());
            if (!string.IsNullOrEmpty(filter.Model_No))
            {
                pred_modelKaizen.And(x => x.model_no.Trim().Contains(filter.Model_No.Trim()));
            }
            var data = await _repoViewModelKaizen.FindAll(pred_modelKaizen)
                .OrderByDescending(x => x.prod_season).ThenBy(x => x.volume).ThenBy(x => x.serial_no)
                .ToListAsync();
            var result = _mapper.Map<List<VW_ModelKaizen>, List<VW_ModelKaizen_Dto>>(data);
            result.ForEach(item =>
            {
                item.start_date_string = item.start_date.ToString("yyyy/MM/dd");
                item.kaizen_type_combine_string = item.kaizen_type_combine == true ? "Y" : "N";
                item.kaizen_type_eliminate_string = item.kaizen_type_eliminate == true ? "Y" : "N";
                item.kaizen_type_reduce_string = item.kaizen_type_reduce == true ? "Y" : "N";
                item.kaizen_type_smart_tool_string = item.kaizen_type_smart_tool == true ? "Y" : "N";
                item.critical_efficiency_string = item.critical_efficiency == true ? "Y" : "N";
                item.critical_quality_string = item.critical_quality == true ? "Y" : "N";
            });
            return result;
        }

        public async Task<List<string>> GetSeasonByUpper(string factory_id, string upper_id)
        {
            var data = await _repoEfficiency.FindAll(x => x.factory_id.Trim() == factory_id.Trim() && x.upper_id.Trim() == upper_id.Trim() && (x.efficiency_target != null || x.efficiency_actual != null))
                .Select(x => x.season.Trim()).Distinct()
                .ToListAsync();
            return data;
        }

        public async Task<PagedList<Model>> Search(PaginationParams param, KaizenReportParam filter, string factory_id)
        {
            var pred_Model = PredicateBuilder.New<Model>(true);
            pred_Model.And(x => x.factory_id.Trim() == factory_id.Trim());
            if (!string.IsNullOrEmpty(filter.Model_No))
            {
                pred_Model.And(x => x.model_no.Trim().Contains(filter.Model_No.Trim()) || x.model_name.Trim().Contains(filter.Model_No.Trim()));
            }
            if (filter.Active != "all")
            {
                pred_Model.And(x => x.is_active == ((filter.Active == "1") ? true : false));
            }
            var models = _repoModel.FindAll(pred_Model).OrderByDescending(x => x.prod_season).ThenBy(x => x.volume);
            return await PagedList<Model>.CreateAsync(models, param.PageNumber, param.PageSize);
        }

        public async Task<bool> UpdateClickTimes(KaizenModelDetail model)
        {
            var kaizen = _repoKaizen.FindSingle(x => x.factory_id.Trim() == model.factory_id.Trim() &&
                                                x.model_no.Trim() == model.model_no.Trim() &&
                                                x.serial_no == model.serial_no &&
                                                x.stage_id.Trim() == model.stage_id.Trim() &&
                                                x.operation_id.Trim() == model.operation_id.Trim());
            kaizen.clicks_times = kaizen.clicks_times + 1;
            try
            {
                return await _repoKaizen.SaveAll();
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}