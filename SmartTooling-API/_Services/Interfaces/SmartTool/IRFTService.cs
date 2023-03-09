using System.Threading.Tasks;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Interfaces.SmartTool
{
    public interface IRFTService
    {
        Task<bool> Add(Measurement_RFTDTO model);
        Task<bool> Update(Measurement_RFTDTO model);

        Task<PagedList<Measurement_RFTDTO>> Search(PaginationParams param, string modelNo, string stage);

        Task<object> GetAllModel();

        Task<object> GetAllStage();

        Task<object> GetAllProcessType(string modelNo, string stage);

        Task<object> GetProcessNOperation(string modelno, string stage, string operation);

        Task<object> GetAllDefectReason();

        Task<object> GetOperationName(string modelNo, string stage, string processtype);
    }
}