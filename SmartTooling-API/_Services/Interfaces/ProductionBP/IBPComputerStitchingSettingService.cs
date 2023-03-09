using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API._Services.Interfaces.ProductionBP
{
    public interface IBPComputerStitchingSettingService
    {
        Task<PagedList<ComputerStitchingSettingViewDTO>> GetAllComputerStitchingSetting(BPSearchComputerStitchingSetting search, PaginationParams pagination);
        Task<OperationResult> CreateComputerStitchingSetting(ComputerStitchingSettingViewDTO model);
        Task<OperationResult> UpdateComputerStitchingSetting(ComputerStitchingSettingViewDTO model);
        Task<List<KeyValuePair<string, string>>> GetModel();
        Task<List<KeyValuePair<string, string>>> GetStage();
        Task<List<ModelOperationDTO>> GetCSOperation(string model_no, string stage_id);
        Task<List<KeyValuePair<string, string>>> GetCSType();
        Task<List<KeyValuePair<string, string>>> GetCSMachineType();
        Task<List<KeyValuePair<string, string>>> GetMainUpperMaterialType();
        Task<List<KeyValuePair<string, string>>> GetJigDesign();
        Task<OperationResult> UploadExcel(IFormFile file, string username, string factory);
    }
}