using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.DTO.BestLine.LayoutDesignOverall;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;

namespace SmartTooling_API._Services.Interfaces.BestLine
{
    public interface ILayoutDesignOverallService
    {
        Task<PagedList<LayoutDesignOverallModelDTO>> Search(PaginationParams pagination, LayoutDesignOverallParam filterParam);
        Task<object> GetLineNoFromBL_Layout_Design_Overall();
        Task<object> GetLineTypeBL_Layout_Design_Overall();
        Task<object> GetAllLineNo();
        Task<object> GetAllLineType();
        Task<object> GetAllModelNo();
        Task<object> GetAllProdSeason();
        Task<OperationResult> AddLayoutDesignOverall(BL_Layout_Design_OverallDTO model);
        Task<bool> UpdateLayoutDesignOverall(BL_Layout_Design_OverallDTO model);
        Task<BL_Layout_Design_OverallDTO> GetParamsEdit(string factory_id, string line_id, string line_type_id, string model_no);

        Task<bool> IsExists(BL_Layout_Design_OverallDTO model);

    }
}