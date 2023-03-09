using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class GroupKaizenReportService : IGroupKaizenReportService
    {
        private readonly IFactoryRepository _repoFactory;
        private readonly IModelRepository _repoModel;
        private readonly IViewModelKaizenRepository _repoViewModelKaizen;
        private readonly IEfficiencyRepository _repoEfficiency;
        private readonly IKaizenRepository _repoKaizen;
        private readonly IModelOperationRepository _repoModelOperation;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IKaizenBenefitsApplicationFormRepository _formRepository;
        private OperationResult operationResult;
        public GroupKaizenReportService(IFactoryRepository repoFactory,
                                            IModelRepository repoModel,
                                            IViewModelKaizenRepository repoViewModelKaizen,
                                            IEfficiencyRepository repoEfficiency,
                                            IKaizenRepository repoKaizen,
                                            IModelOperationRepository repoModelOperation,
                                            IMapper mapper,
                                            IConfiguration configuration,
                                            IKaizenBenefitsApplicationFormRepository formRepository
                                            )
        {
            _repoFactory = repoFactory;
            _repoModel = repoModel;
            _repoViewModelKaizen = repoViewModelKaizen;
            _repoEfficiency = repoEfficiency;
            _repoKaizen = repoKaizen;
            _repoModelOperation = repoModelOperation;
            _mapper = mapper;
            _configuration = configuration;
            _formRepository = formRepository;
        }
        public async Task<List<Factory>> GetAllFactory()
        {
            var data = await _repoFactory.GetAll().ToListAsync();
            return data;
        }

        public async Task<PagedList<Model>> Search(PaginationParams param, KaizenReportGroupParam filterParam)
        {
            var pred_Model = PredicateBuilder.New<Model>(true);
            _configuration.GetSection("AppSettings:DataSeach").Value = filterParam.factory_id.Trim();
            if (!string.IsNullOrEmpty(filterParam.factory_id))
            {
                pred_Model.And(x => x.factory_id.Trim() == filterParam.factory_id.Trim());
            }
            if (!string.IsNullOrEmpty(filterParam.model_no))
            {
                pred_Model.And(x => x.model_no.Trim().Contains(filterParam.model_no.Trim()) || x.model_name.Trim().Contains(filterParam.model_no.Trim()));
            }
            if (filterParam.active != "all")
            {
                pred_Model.And(x => x.is_active == ((filterParam.active == "1") ? true : false));
            }
            var data = _repoModel.FindAll(pred_Model);
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return await PagedList<Model>.CreateAsync(data, param.PageNumber, param.PageSize);
        }
        public async Task<List<VW_ModelKaizen_Dto>> GetModelKaizens(KaizenReportGroupParam filterParam)
        {
            var pred_model_kaizen = PredicateBuilder.New<VW_ModelKaizen>(true);
            _configuration.GetSection("AppSettings:DataSeach").Value = filterParam.factory_id.Trim();
            if (!string.IsNullOrEmpty(filterParam.factory_id))
            {
                pred_model_kaizen.And(x => x.factory_id.Trim() == filterParam.factory_id.Trim());
            }
            if (!string.IsNullOrEmpty(filterParam.model_no))
            {
                pred_model_kaizen.And(x => x.model_no.Trim().Contains(filterParam.model_no.Trim()));
            }
            var data = await _repoViewModelKaizen.FindAll(pred_model_kaizen)
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
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return result;
        }

        public async Task<Model> GetModelByModelNo(string factory_id, string model_No)
        {
            _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
            var model = await _repoModel.FindAll(x => x.factory_id.Trim() == factory_id.Trim() &&
                                                 x.model_no.Trim() == model_No.Trim()).FirstOrDefaultAsync();
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return model;
        }

        public async Task<List<string>> GetSeasonByUpper(string factory_id, string upper_id)
        {
            _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
            var data = await _repoEfficiency.FindAll(x => x.factory_id.Trim() == factory_id.Trim() && x.upper_id.Trim() == upper_id.Trim() && (x.efficiency_target != null || x.efficiency_actual != null))
                .Select(x => x.season.Trim()).Distinct()
                .ToListAsync();
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return data;
        }

        public async Task<List<Efficiency>> GetEfficiencys(string factory_id, string upper_id, string season)
        {
            _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
            var data = await _repoEfficiency.FindAll(x => x.factory_id.Trim() == factory_id.Trim() &&
                            x.upper_id.Trim() == upper_id.Trim() &&
                            x.season.Trim() == season.Trim()).OrderBy(x => x.sequence).ToListAsync();
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return data;
        }

        public async Task<PagedList<KaizenModelDetail>> GetKaiZens(PaginationParams param, string factory_id, string model_no)
        {
            _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
            var kaizens = _repoKaizen.FindAll(x => x.factory_id.Trim() == factory_id.Trim() &&
                               x.model_no.Trim() == model_no.Trim())
                    .OrderBy(x => x.serial_no);
            var modelOperations = _repoModelOperation.FindAll(x => x.factory_id.Trim() == factory_id.Trim() &&
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
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return await PagedList<KaizenModelDetail>.CreateAsync(data, param.PageNumber, param.PageSize);
        }
        public async Task<object> GetKaizenDetail(string factory_id, string model_no, string serial_no)
        {
            _configuration.GetSection("AppSettings:DataSeach").Value = factory_id.Trim();
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
            _configuration.GetSection("AppSettings:DataSeach").Value = "";
            return data;
        }
        public async Task<OperationResult> AddCross(Kaizen_Benefits_Application_FormDTO model)
        {

            var models = _mapper.Map<Kaizen_Benefits_Application_Form>(model);
            models.create_time = DateTime.Now;
            models.update_time = DateTime.Now;
            if (await _formRepository.CheckKaizenDescriptionExist(model, models.factory_id))
            {
                return operationResult = new OperationResult { Caption = "Fail", Message = "This Cross Site Sharing is Exist", Success = false };
            }
            else
            {
                _formRepository.Add(models);
                await _formRepository.SaveAll();
                return operationResult = new OperationResult { Caption = "Success", Message = "Add Cross Site Sharing Success", Success = true };
            }
        }

    }
}