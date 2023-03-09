

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;

namespace SmartTooling_API._Services.Interfaces.ProductionBP
{
    public interface IPadPrintSettingService
    {
        Task<PagedList<PBP_Pad_Print_Setting_DTO>> Search(PadPrintSettingParam filterParam, PaginationParams pagination);
        Task<List<KeyValuePair<string, string>>> GetAllModel();
        Task<List<KeyValuePair<string, string>>> GetAllPadShape();
        Task<List<KeyValuePair<string, string>>> GetAllMachineVendor();
        Task<List<KeyValuePair<string, string>>> GetAllMaterialType();
        Task<PBP_Pad_Print_SettingDTO> GetDetail(PBP_Pad_Print_Setting_DTO model);
        Task<OperationResult> AddNew(PBP_Pad_Print_SettingDTO model);
        Task<OperationResult> Update(PBP_Pad_Print_SettingDTO model);
        Task<bool> FindPadPrintSetting(PBP_Pad_Print_SettingDTO model);
        Task<OperationResult> UploadExcel(IFormFile file, string user);
    }
}