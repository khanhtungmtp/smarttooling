using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Interfaces.SmartTool
{
    public interface IModelOperationService
    {
        Task<PageListUtility<ModelOperationDTO>> SearchModelOperation(PaginationParams param, ModelOperationParam modelParam);

        Task<bool> Add(ModelOperationDTO model);

        Task<bool> Update(ModelOperationDTO model);

        // Task<bool> Delete(ModelOperationDTO operation);

        Task<ModelOperationDTO> GetModelOperation(ModelOperationEditParam modelOperationEditParam);

        Task<object> GetAllProcessType();

        //  Task<bool> CheckModelOperationIsUsing(ModelOperationDTO operation);

        Task<List<ModelOperationDTO>> GetExportExcelData(ModelOperationParam param);

        Task<OperationResult> UploadExcel(IFormFile file, string user);
    }
}