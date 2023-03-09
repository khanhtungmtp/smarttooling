using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Interfaces.SmartTool
{
    public interface IModelEfficiencyService
    {
        Task<bool> UpdateOrInsert(List<ModelEfficiencyDTO> modelEfficiencyDTO, string factory, string username);

        Task<object> GetAllUpperID();

        Task<object> GetModelName(string upperId, string factory);

        Task<List<ModelEfficiencyDTO>> ModelEfficiencyEdit(ModelEfficiencyEditParam modelParam);

        Task<bool> CheckExist(ModelEfficiencyEditParam modelParam);


    }
}