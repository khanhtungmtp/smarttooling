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
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Services.Services.SmartTool
{
    public class ModelEfficiencyService : IModelEfficiencyService
    {
        private readonly IEfficiencyRepository _repoModelEficiency;
        private readonly IModelRepository _repoModel;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private string factory;
        public ModelEfficiencyService(IEfficiencyRepository repoModelEficiency,
                                        IModelRepository repoModel,
                                        IMapper mapper,
                                        MapperConfiguration configMapper,
                                        IConfiguration configuration)
        {
            _repoModelEficiency = repoModelEficiency;
            _repoModel = repoModel;
            _mapper = mapper;
            _configMapper = configMapper;
            factory = configuration.GetSection("AppSettings:Factory").Value;
        }

        public async Task<bool> UpdateOrInsert(List<ModelEfficiencyDTO> listModelEfficiencyDTO, string factory, string username)
        {
            foreach (var efficiency in listModelEfficiencyDTO)
            {
                efficiency.season = efficiency.season + efficiency.season_year;
                var effParam = new ModelEfficiencyEditParam();
                effParam.factory = factory;
                effParam.season = efficiency.season;
                effParam.month = efficiency.month;
                effParam.upper_id = efficiency.upper_id;
                var modelEfficiency = _mapper.Map<Efficiency>(efficiency);
                modelEfficiency.update_time = DateTime.Now;
                modelEfficiency.update_by = username;
                modelEfficiency.factory_id = factory;
                if (await CheckExist(effParam))
                {
                    _repoModelEficiency.Update(modelEfficiency);
                }
                else
                {
                    modelEfficiency.create_time = DateTime.Now;
                    modelEfficiency.create_by = username;
                    _repoModelEficiency.Add(modelEfficiency);
                }
            }
            return await _repoModelEficiency.SaveAll();
        }

        public async Task<object> GetAllUpperID()
        {
            return await _repoModel.FindAll(x => x.factory_id.Trim() == factory && x.is_active == true)
                                            .OrderBy(x => x.upper_id)
                                            .Select(x => new { x.upper_id }).Distinct().ToListAsync();
        }

        public async Task<object> GetModelName(string upperId, string factory)
        {
            return await _repoModel.FindAll(x => x.factory_id.Trim() == factory &&
                                            x.upper_id.Trim() == upperId.Trim())
                                            .OrderBy(x => x.model_name)
                                            .Select(x => new { Name = x.model_name })
                                            .Distinct().ToListAsync();
        }

        public async Task<List<ModelEfficiencyDTO>> ModelEfficiencyEdit(ModelEfficiencyEditParam modelParam)
        {
            var pred_ModelEfficiency = PredicateBuilder.New<Efficiency>(true);
            if (!String.IsNullOrEmpty(modelParam.season))
            {
                pred_ModelEfficiency.And(x => x.factory_id.Trim() == factory && x.season.Contains(modelParam.season));
            }
            if (!String.IsNullOrEmpty(modelParam.upper_id))
            {
                pred_ModelEfficiency.And(x => x.factory_id.Trim() == factory && x.upper_id == modelParam.upper_id);
            }
            var list = await _repoModelEficiency.FindAll(pred_ModelEfficiency).OrderBy(x => x.sequence).ToListAsync();
            var modelEfficiency = _mapper.Map<List<ModelEfficiencyDTO>>(list);
            return modelEfficiency;
        }

        public async Task<bool> CheckExist(ModelEfficiencyEditParam effParam)
        {
            return await _repoModelEficiency.CheckExists(effParam);
        }
    }
}