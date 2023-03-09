using System.Threading.Tasks;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Interfaces.SmartTool
{
    public interface IModelService : IMainService<ModelDTO>
    {
        Task<PagedList<ModelDTO>> SearchModel(PaginationParams param, ModelParam modelParam);

        Task<object> GetModelType();

        Task<ModelDTO> GetByFactoryAndModelNo(string facID, string modelNo);

        Task<PageListUtility<ModelExcelDto>> ExportExcel(PaginationParams param, ModelParam modelParam);

        Task<OperationResult> UploadExcel(IFormFile file, string username);
    }
}