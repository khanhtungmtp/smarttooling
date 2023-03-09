using System.Threading.Tasks;
using SmartTooling_API.Helpers.Params.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.SmartTool
{
    public interface IModelOperationRepository : IMainRepository<Model_Operation>
    {
        Task<Model_Operation> GetByModelOperation(ModelOperationEditParam modelParam);
    }
}