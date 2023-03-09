using System.Threading.Tasks;
using SmartTooling_API.Models.SmartTool;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.SmartTool
{
    public interface IKaizenRepository : IMainRepository<Kaizen>
    {
        Task<bool> CheckExistsKaizen(ModelOperationDTO operation);

        Task<bool> CheckKaizenDescriptionExist(KaizenDTO model);
    }
}