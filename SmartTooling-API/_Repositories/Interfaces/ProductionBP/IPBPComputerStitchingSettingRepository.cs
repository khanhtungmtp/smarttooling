using SmartTooling_API.Models.ProductionBP;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.ProductionBP
{
    public interface IPBPComputerStitchingSettingRepository : IProductionBPRepository<PBP_ComputerStitchingSetting>
    {
        Task<bool> CheckExistsPBPComputerStitchingSetting(ModelOperationDTO operation);
    }
}