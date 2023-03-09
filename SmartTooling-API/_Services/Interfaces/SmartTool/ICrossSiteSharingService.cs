
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Interfaces.SmartTool
{
    public interface ICrossSiteSharingService
    {
        Task<PagedList<CrossSiteSharingDTO>> Search(PaginationParams param, CrossSiteSharingParam filterParam);
        Task<CrossSiteSharingEditDTO> GetCrossSiteSharingEdit(string factory, string modelNO, string serialNo);
        Task<List<CrossSiteSharingEditDTO>> GetCrossSiteSharingPDF(List<CrossSiteSharingDTO> filterParam);
        Task<OperationResult> UpdateCrossSiteSharing(Kaizen_Benefits_Application_FormDTO model);
    }
}