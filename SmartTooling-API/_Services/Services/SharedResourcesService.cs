using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.BestLine;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API._Services.Interfaces;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Services.Services
{
    public class SharedResourcesService : ISharedResourcesService
    {
        private readonly IFactoryRepository _factoryRepo;
        private readonly IConfiguration _configuration;
        private readonly IBLLinesRepository _linesBLRepo;
        private readonly IBLLineTypeRepository _lineTypeBLRepo;
        private readonly IModelRepository _modelRepo;
        private readonly IStageRepository _stageRepo;
        public SharedResourcesService(  IFactoryRepository factoryRepo,
                                        IBLLinesRepository linesBLRepo,
                                        IBLLineTypeRepository lineTypeBLRepo,
                                        IStageRepository stageRepo,
                                        IModelRepository modelRepo,
                                        IConfiguration configuration) {
            _factoryRepo = factoryRepo;
            _linesBLRepo = linesBLRepo;
            _lineTypeBLRepo = lineTypeBLRepo;
            _stageRepo = stageRepo;
            _modelRepo = modelRepo;
            _configuration = configuration;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllFactory()
        {
            return await _factoryRepo.FindAll().Select(x => new KeyValuePair<string, string>(
                    x.factory_id.Trim(),
                    x.factory_name.Trim()
                )).ToListAsync();
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllLine(string factory)
        {
            if(!string.IsNullOrEmpty(factory)) {
             _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
            }
            var result = await _linesBLRepo.FindAll().OrderBy(x => x.sequence)
                .Select(x => new KeyValuePair<string, string>(
                    x.line_id.Trim(),
                    x.line_name.Trim()
                )).Distinct().ToListAsync();
            if(!string.IsNullOrEmpty(factory)) {
                _configuration.GetSection("AppSettings:DataSeach").Value = "";
            }
            return result;
        }

        public async Task<List<KeyValuePair<string, string>>> GetAllLineType(string factory)
        {
            if(!string.IsNullOrEmpty(factory)) {
                _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
            }
            var result = await _lineTypeBLRepo.FindAll().OrderBy(x => x.sequence)
                .Select(x => new KeyValuePair<string, string>(
                    x.line_type_id.Trim(),
                    x.line_type_name.Trim()
                )).Distinct().ToListAsync();
            if(!string.IsNullOrEmpty(factory)) {
                _configuration.GetSection("AppSettings:DataSeach").Value = "";
            }
            return result;
        }

        public async Task<List<Model_List_DTO>> GetAllModel()
        {
            var data = await _modelRepo.FindAll(x => x.is_active == true).Select(x => new Model_List_DTO{
                Model_No = x.model_no.Trim(),
                Model_Name = x.model_name.Trim(),
                Dev_Season = x.dev_season.Trim(),
                Prod_Season = x.prod_season.Trim()
            }).Distinct().ToListAsync();
            return data;
        }

        public async Task<List<KeyValuePair<string, string>>> GetStage(string factory)
        {
            if(!string.IsNullOrEmpty(factory)) {
                _configuration.GetSection("AppSettings:DataSeach").Value = factory.Trim();
            }
            var result = await _stageRepo.FindAll(x => x.is_active == true)
                .Distinct().OrderBy(x => x.sequence)
                .Select(x => new KeyValuePair<string, string>(
                    x.stage_id.Trim(),
                    x.stage_name.Trim()
                )).ToListAsync();
            if(!string.IsNullOrEmpty(factory)) {
                _configuration.GetSection("AppSettings:DataSeach").Value = "";
            }
            return result;
        }
    }
}