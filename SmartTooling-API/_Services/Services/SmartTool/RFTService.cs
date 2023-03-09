using System;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces.SmartTool;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Models.SmartTool;
using SmartTooling_API.Helpers.Params;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class RFTService : IRFTService
    {
        private readonly IStageRepository _iStageRepo;
        private readonly IMeasurement_RFTRepository _iMeasurementRepo;
        private readonly IModelOperationRepository _iModelOperaRepo;
        private readonly IProcessTypeRepository _repoProcessType;
        private readonly IModelRepository _iModelRepo;
        private readonly IDefectReasonRepository _iDefectReasonRepo;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private string factory;

        public RFTService(IStageRepository iStageRepo,
                            IMeasurement_RFTRepository iMeasurementRepo,
                            IModelOperationRepository iModelOperaRepo,
                            IModelRepository iModelRepo, IMapper mapper,
                            MapperConfiguration configMapper,
                            IConfiguration configuration,
                            IProcessTypeRepository repoProcessType,
                            IDefectReasonRepository iDefectReasonRepo)
        {
            _iStageRepo = iStageRepo;
            _iMeasurementRepo = iMeasurementRepo;
            _iModelOperaRepo = iModelOperaRepo;
            _iModelRepo = iModelRepo;
            _iDefectReasonRepo = iDefectReasonRepo;
            _mapper = mapper;
            _configMapper = configMapper;
            factory = configuration.GetSection("AppSettings:Factory").Value;
            _repoProcessType = repoProcessType;
        }

        public async Task<object> GetAllModel()
        {
            return await _iModelRepo.FindAll(x => x.factory_id == factory && x.is_active)
                                            .OrderByDescending(x => x.prod_season).OrderByDescending(x => x.volume)
                                            .Select(x => new { x.model_no, x.model_name, x.upper_id })
                                            .Distinct().ToListAsync();
        }

        public async Task<object> GetAllStage()
        {
            var model = _iStageRepo.FindAll(x => x.factory_id == factory && x.is_active)
                                            .Select(x => new { x.stage_id, x.stage_name, x.sequence })
                                            .Distinct();
            var model2 = await model.OrderBy(x => x.sequence).ToListAsync();
            return model2;
        }

        public async Task<object> GetAllProcessType(string modelNo, string stage)
        {
            var MOp = _iModelOperaRepo.FindAll(x => x.critical_quality && x.factory_id == factory && x.model_no == modelNo && x.stage_id == stage);
            var MRFT = _iMeasurementRepo.FindAll(x => x.factory_id == factory && x.model_no == modelNo && x.stage_id == stage);
            var process = _repoProcessType.FindAll(x => x.factory_id == factory && x.is_active);

            var oprationdata = await (from T1 in MOp
                                      join T2 in MRFT on new { Factory = T1.factory_id, model_no = T1.model_no, stage_id = T1.stage_id, operation_id = T1.operation_id }
                                      equals new { Factory = T2.factory_id, model_no = T2.model_no, stage_id = T2.stage_id, operation_id = T2.operation_id }
                                      into groupjoin
                                      from a in groupjoin.DefaultIfEmpty()
                                      orderby T1.sequence
                                      select new { t2op = a.operation_id })
                                        .Where(x => x.t2op == null).ToListAsync();
            if (oprationdata.Count == 0) // �L��ƪ�^0
            {
                return oprationdata;
            }
            else // ��^process���
            {
                var prodata = await (from T1 in MOp
                                     join T2 in process on new { Factory = T1.factory_id, process_type_id = T1.process_type_id }
                                     equals new { Factory = T2.factory_id, process_type_id = T2.process_type_id }
                                     into groupjoin
                                     from a in groupjoin.DefaultIfEmpty()
                                     select new { T1.process_type_id, a.process_type_name_local })
                                .Distinct().ToListAsync();
                return prodata;
            }
            //return await _repoProcessType.FindAll().Where(x => x.factory_id == factory && x.is_active)
            //                                .OrderBy(x => x.sequence)
            //                               .Select(x => new { x.process_type_id, x.process_type_name_local }).Distinct().ToListAsync();
        }

        public async Task<object> GetProcessNOperation(string modelno, string stage, string operation)
        {
            var MOp = _iModelOperaRepo.FindAll(x => x.critical_quality && x.factory_id == factory && x.model_no == modelno && x.stage_id == stage && x.operation_id == operation);
            var process = _repoProcessType.FindAll().Where(x => x.factory_id == factory && x.is_active);
            var data = await (from T1 in MOp
                              join T2 in process on T1.process_type_id equals T2.process_type_id
                              into groupjoin
                              from B in groupjoin.DefaultIfEmpty()
                              select new
                              {
                                  T1.operation_id,
                                  T1.operation_name_local,
                                  B.process_type_id,
                                  B.process_type_name_local
                              }).ToListAsync();
            return data;
        }

        public async Task<object> GetAllDefectReason()
        {
            return await _iDefectReasonRepo.FindAll(x => x.factory_id == factory && x.is_active)
                .OrderBy(x => x.sequence)
                .Select(x => new { x.defect_reason_id, x.defect_reason_name })
                .ToListAsync();
        }

        public async Task<object> GetOperationName(string modelNo, string stage, string processtype)
        {
            var MOp = _iModelOperaRepo.FindAll(x => x.critical_quality && x.factory_id == factory && x.model_no == modelNo && x.stage_id == stage && x.process_type_id == processtype);
            var MRFT = _iMeasurementRepo.FindAll(x => x.factory_id == factory && x.model_no == modelNo && x.stage_id == stage);

            var data = await (from T1 in MOp
                              join T2 in MRFT on new { Factory = T1.factory_id, model_no = T1.model_no, stage_id = T1.stage_id, operation_id = T1.operation_id }
                              equals new { Factory = T2.factory_id, model_no = T2.model_no, stage_id = T2.stage_id, operation_id = T2.operation_id }
                              into groupjoin
                              from a in groupjoin.DefaultIfEmpty()
                                  //where T2.operation_id == null
                              orderby T1.sequence
                              select new { T1.operation_id, T1.operation_name_local, t2op = a.operation_id })
                                .Where(x => x.t2op == null)
                                .ToListAsync();

            return data;
            //return await _iModelOperaRepo.FindAll().Where(x => x.critical_quality && x.factory_id == factory && x.model_no == modelNo && x.stage_id == stage && x.process_type_id == processtype)
            //    .OrderBy(x => x.sequence)
            //    .Select(x => new { x.operation_id, x.operation_name_local }).ToListAsync();
        }

        public async Task<PagedList<Measurement_RFTDTO>> Search(PaginationParams param, string modelNo, string stage)
        {
            var queryMeasurement = _iMeasurementRepo.FindAll(x => x.factory_id == factory);
            var queryModelOpera = _iModelOperaRepo.FindAll();
            if (!String.IsNullOrEmpty(modelNo))
                queryMeasurement = queryMeasurement.Where(x => x.model_no == modelNo);

            if (!String.IsNullOrEmpty(stage))
                queryMeasurement = queryMeasurement.Where(x => x.stage_id == stage);

            var dataGroup = queryMeasurement
                .Join(queryModelOpera,
                        x => new { x.factory_id, x.model_no, x.operation_id },
                        y => new { y.factory_id, y.model_no, y.operation_id }, (x, y) => x)
                        .ProjectTo<Measurement_RFTDTO>(_configMapper);
            return await PagedList<Measurement_RFTDTO>.CreateAsync(dataGroup, param.PageNumber, param.PageSize);
        }

        public async Task<bool> Add(Measurement_RFTDTO model)
        {
            var measurement = _mapper.Map<Measurement_RFT>(model);
            _iMeasurementRepo.Add(measurement);
            return await _iMeasurementRepo.SaveAll();
        }

        public async Task<bool> Update(Measurement_RFTDTO model)
        {
            var measurement = _mapper.Map<Measurement_RFT>(model);
            _iMeasurementRepo.Update(measurement);
            return await _iMeasurementRepo.SaveAll();
        }
    }
}