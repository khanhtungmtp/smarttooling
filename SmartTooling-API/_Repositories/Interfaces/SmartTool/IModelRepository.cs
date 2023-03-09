using System.Threading.Tasks;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.SmartTool
{
    public interface IModelRepository : IMainRepository<Model>
    {
        Task<Model> GetByFactoryAndModelNo(string facID, string modelNo);
    }
}