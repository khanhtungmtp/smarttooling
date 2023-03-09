using System.Threading.Tasks;
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.SmartTool
{
    public interface IEfficiencyRepository : IMainRepository<Efficiency>
    {
        Task<bool> CheckExists(ModelEfficiencyEditParam editParam);
    }
}