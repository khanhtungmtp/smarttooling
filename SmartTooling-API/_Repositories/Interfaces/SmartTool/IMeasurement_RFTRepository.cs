using System.Threading.Tasks;
using SmartTooling_API.Models.SmartTool;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.SmartTool
{
    public interface IMeasurement_RFTRepository : IMainRepository<Measurement_RFT>
    {
        Task<bool> CheckExistsRTF(ModelOperationDTO operation);
    }
}