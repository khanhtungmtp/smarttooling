using System.Threading.Tasks;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Interfaces.SmartTool
{
    public interface IDefectReasonService : IMainService<DefectReasonDTO>
    {
        Task<bool> CheckDefectReasonExists(string defectreasonID);

        Task<PagedList<DefectReasonDTO>> SearchDefectReason(PaginationParams paginationParams, DefectReasonParam defectReasonParam);
    }
}