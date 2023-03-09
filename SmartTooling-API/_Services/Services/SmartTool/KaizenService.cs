using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using System.Linq;
using SmartTooling_API.Models.SmartTool;
using System;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API.Helpers.Params;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class KaizenService : IKaizenService
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IKaizenRepository _kaizen;
        private readonly IModelRepository _modelRepository;
        private readonly IStageRepository _stageRepository;
        private readonly IFactoryRepository _factoryRepository;
        private readonly IModelOperationRepository _modelOperationRepository;
        private readonly IProcessTypeRepository _processTypeRepository;
        private OperationResult operationResult;

        public KaizenService(IMapper mapper,
                                MapperConfiguration configMapper,
                                IKaizenRepository kaizen,
                                IModelRepository modelRepository,
                                IStageRepository stageRepository,
                                IFactoryRepository factoryRepository,
                                IModelOperationRepository modelOperationRepository,
                                IProcessTypeRepository processTypeRepository)
        {
            _mapper = mapper;
            _configMapper = configMapper;
            _kaizen = kaizen;
            _modelRepository = modelRepository;
            _stageRepository = stageRepository;
            _factoryRepository = factoryRepository;
            _modelOperationRepository = modelOperationRepository;
            _processTypeRepository = processTypeRepository;
        }
        public Task<bool> Add(KaizenDTO model)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(KaizenDTO model)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<KaizenDTO>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public KaizenDTO GetById(object id)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<KaizenDTO>> GetWithPaginations(PaginationParams param)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<KaizenDTO>> Search(PaginationParams param, object text)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PagedList<KaizenDTO>> Search(PaginationParams param, string model_no, string factory)
        {
            var model = _modelOperationRepository.FindAll().Where(x => x.factory_id == factory);
            var Kaizen = _kaizen.FindAll().Where(x => x.factory_id == factory);
            // if (!String.IsNullOrEmpty(model_no))
            Kaizen = Kaizen.Where(x => x.model_no == model_no);
            // var data = Kaizen.ProjectTo<KaizenDTO>(_configMapper).OrderBy(x=>x.serial_no);
            var data = (from a in Kaizen
                        join b in model on new
                        {
                            Factory = a.factory_id,
                            model_no = a.model_no,
                            Stage = a.stage_id
                        ,
                            opera = a.operation_id
                        }
                 equals new { Factory = b.factory_id, model_no = b.model_no, Stage = b.stage_id, opera = b.operation_id }
                        select new KaizenDTO()
                        {
                            factory_id = a.factory_id,
                            model_no = a.model_no,
                            serial_no = a.serial_no,
                            kaizen_description = a.kaizen_description,
                            stage_id = a.stage_id,
                            operation_id = a.operation_id,
                            start_date = a.start_date,
                            kaizen_type_eliminate = a.kaizen_type_eliminate,
                            kaizen_type_combine = a.kaizen_type_combine,
                            kaizen_type_reduce = a.kaizen_type_reduce,
                            kaizen_type_smart_tool = a.kaizen_type_smart_tool,
                            process_tct_sec = a.process_tct_sec,
                            ct_before_sec = a.ct_before_sec,
                            ct_after_sec = a.ct_after_sec,
                            rft_before_percent = a.rft_before_percent,
                            rft_after_percent = a.rft_after_percent,
                            line_roll_out_percent = a.line_roll_out_percent,
                            before_media = a.before_media,
                            after_media = a.after_media,
                            before_remarks = a.before_remarks,
                            after_remarks = a.after_remarks,
                            kaizen_from = a.kaizen_from,
                            clicks_times = a.clicks_times,
                            create_by = a.create_by,
                            update_by = a.update_by,
                            create_time = a.create_time,
                            update_time = a.update_time,
                            process = b.process_type_id
                        }).OrderBy(x => x.serial_no);
            return await PagedList<KaizenDTO>.CreateAsync(data, param.PageNumber, param.PageSize);
        }

        public async Task<OperationResult> AddKaizen(KaizenDTO model)
        {
            if (await _kaizen.CheckKaizenDescriptionExist(model))
            {
                return operationResult = new OperationResult { Caption = "Fail", Message = "This Kaizen Description is Exist", Success = false };
            }
            else
            {
                var models = _mapper.Map<Kaizen>(model);
                models.create_time = DateTime.Now;
                _kaizen.Add(models);
                await _kaizen.SaveAll();
                return operationResult = new OperationResult { Caption = "Success", Message = "Add Kaizen Success", Success = true };
            }
        }

        public async Task<OperationResult> UpdateKaizen(KaizenDTO model)
        {
            if (await _kaizen.CheckKaizenDescriptionExist(model))
            {
                return operationResult = new OperationResult { Caption = "Fail", Message = "This Kaizen Description is Exist", Success = false };
            }
            else
            {
                model.update_time = DateTime.Now;
                var modelUp = _mapper.Map<Kaizen>(model);
                _kaizen.Update(modelUp);
                await _kaizen.SaveAll();
                return operationResult = new OperationResult { Caption = "Success", Message = "Update Kaizen Success", Success = true };
            }
        }
        public async Task<object> GetModelNo(string factory)
        {
            var Model_no = await _modelRepository.FindAll(x => x.factory_id == factory && x.is_active == true).ToListAsync();
            return Model_no.OrderByDescending(x => x.prod_season).ThenByDescending(x => x.volume);
        }
        public async Task<object> GetKaizenFrom()
        {
            var kaizen = await _factoryRepository.FindAll().ToListAsync();
            return kaizen.OrderBy(x => x.factory_id);
        }
        public async Task<object> GetStage(string factory)
        {
            var stages = await _stageRepository.FindAll(x => x.factory_id == factory && x.is_active == true).ToListAsync();
            return stages.OrderBy(x => x.sequence);
        }
        public async Task<object> Getprocess(string modelNO, string stage, string factory)
        {
            var Process = _processTypeRepository.FindAll(x => x.factory_id == factory);
            var Opera = _modelOperationRepository.FindAll(x => x.factory_id == factory);
            if (!String.IsNullOrEmpty(modelNO))
                Opera = Opera.Where(x => x.model_no == modelNO);
            if (!String.IsNullOrEmpty(stage))
                Opera = Opera.Where(x => x.stage_id == stage);
            var data = await (from a in Process
                              join b in Opera on new { Factory = a.factory_id, Process_id = a.process_type_id } equals new { Factory = b.factory_id, Process_id = b.process_type_id }
                              select new
                              {
                                  b.process_type_id,
                                  a.process_type_name_en,
                                  a.sequence
                              }).Distinct().OrderBy(x => x.sequence).ToListAsync();
            return data;
        }
        public async Task<object> Getopera(string modelNO, string stage, string process, string factory)
        {
            var Opera = _modelOperationRepository.FindAll(x => x.factory_id == factory);
            if (!String.IsNullOrEmpty(modelNO))
                Opera = Opera.Where(x => x.model_no == modelNO);
            if (!String.IsNullOrEmpty(stage))
                Opera = Opera.Where(x => x.stage_id == stage);
            if (!String.IsNullOrEmpty(process))
                Opera = Opera.Where(x => x.process_type_id == process);
            return await Opera.OrderBy(x => x.sequence).ToListAsync();
        }
        public async Task<KaizenDTO> GetbyID(string modelNO)
        {
            var data = await _kaizen.FindAll(x => x.model_no == modelNO).OrderByDescending(x => x.serial_no).ToListAsync();
            return _mapper.Map<List<KaizenDTO>>(data).FirstOrDefault();
        }
        public async Task<KaizenDTO> GetKaizenEdit(string modelNo, string serialNo, string factory)
        {
            var model = _modelOperationRepository.FindAll().Where(x => x.factory_id == factory && x.model_no == modelNo);
            var Kaizen = _kaizen.FindAll().Where(x => x.factory_id == factory && x.model_no == modelNo
           && x.serial_no == serialNo.ToInt());
            var data = await (from a in Kaizen
                              join b in model on new
                              {
                                  Factory = a.factory_id,
                                  model_no = a.model_no,
                                  Stage = a.stage_id
                              ,
                                  opera = a.operation_id
                              }
                      equals new { Factory = b.factory_id, model_no = b.model_no, Stage = b.stage_id, opera = b.operation_id }
                              select new KaizenDTO()
                              {
                                  factory_id = a.factory_id,
                                  model_no = a.model_no,
                                  serial_no = a.serial_no,
                                  kaizen_description = a.kaizen_description,
                                  stage_id = a.stage_id,
                                  operation_id = a.operation_id,
                                  start_date = a.start_date,
                                  kaizen_type_eliminate = a.kaizen_type_eliminate,
                                  kaizen_type_combine = a.kaizen_type_combine,
                                  kaizen_type_reduce = a.kaizen_type_reduce,
                                  kaizen_type_smart_tool = a.kaizen_type_smart_tool,
                                  process_tct_sec = a.process_tct_sec,
                                  ct_before_sec = a.ct_before_sec,
                                  ct_after_sec = a.ct_after_sec,
                                  rft_before_percent = a.rft_before_percent,
                                  rft_after_percent = a.rft_after_percent,
                                  line_roll_out_percent = a.line_roll_out_percent,
                                  before_media = a.before_media,
                                  after_media = a.after_media,
                                  before_remarks = a.before_remarks,
                                  after_remarks = a.after_remarks,
                                  kaizen_from = a.kaizen_from,
                                  clicks_times = a.clicks_times,
                                  create_by = a.create_by,
                                  update_by = a.update_by,
                                  create_time = a.create_time,
                                  update_time = a.update_time,
                                  process = b.process_type_id
                              }).OrderBy(x => x.serial_no).ToListAsync();
            return _mapper.Map<List<KaizenDTO>>(data).FirstOrDefault();
        }
    }
}