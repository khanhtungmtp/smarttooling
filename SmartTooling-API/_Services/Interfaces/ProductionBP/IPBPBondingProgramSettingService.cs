using Microsoft.AspNetCore.Http;
using SmartTooling_API.DTO.ProductionBP;
using SmartTooling_API.DTO.ProductionBP.BondingProgramSetting;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.ProductionBP;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartTooling_API._Services.Interfaces.ProductionBP
{
    public interface IPBPBondingProgramSettingService
    {
        //Task<PagedList<LayoutDesignOverallModelDTO>> Search(PaginationParams pagination, LayoutDesignOverallParam filterParam);

        //Task<List<string>> GetAllProdSeason(string factory_id);

        //Task<List<KeyValuePair<string, string>>> GetAllModel(string factory_id);

        Task<List<KeyValuePair<string, string>>> GetAllChemicalProcessType();

        Task<List<KeyValuePair<string, string>>> GetAllAutoTech();

        Task<List<KeyValuePair<string, string>>> GetAllModelNo();

        Task<List<KeyValuePair<string, string>>> GetAllAdoptionComponentType();

        Task<List<KeyValuePair<string, string>>> GetAllProcessAdoptionScopeType();

        Task<List<KeyValuePair<string, string>>> GetAllMainUpperMaterialType();

        Task<List<KeyValuePair<string, string>>> GetAllMainBottomMaterialType();

        Task<List<KeyValuePair<string, string>>> GetAllChemicalSupplier();

        Task<PagedList<BondingProgramSettingModelDTO>> Search(PaginationParams pagination, BondingProgramSettingParam filterParam);

        Task<bool> AddBondingProgramSetting(PBP_Bonding_Program_SettingDTO model);

        Task<bool> UpdateBondingProgramSetting(PBP_Bonding_Program_SettingDTO model);

        Task<OperationResult> UploadExcel(IFormFile file, string user);

        Task<bool> IsExists(PBP_Bonding_Program_SettingDTO model);

        //Task<PagedList<PBP_Chemical_Process_TypeDTO>> Search(PaginationParams pagination, C2BOverallLayoutReportParam searchParam);
        //Task<PBP_Chemical_Process_TypeDTO> GetDetailDTO(C2BOverallLayoutReportParam searchParam);
        //Task<PagedList<C2BOverallLayoutReportFilesDTO>> GetFilesDTO(PaginationParams pagination, C2BOverallLayoutReportParam searchParam);
    }
}
