using SmartTooling_API.DTO.BestLine;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.BestLine;

namespace SmartTooling_API._Services.Interfaces.BestLine
{
    public interface IC2BLayoutAttachmentService
    {
        Task<PagedList<C2B_Layout_AttachmentDTO>> Search(PaginationParams param, C2BLayoutAttachmentParam filterParam);
        Task<object> GetAllLineNo();
        Task<object> GetAllLineType();
        Task<object> GetAllProdSeason();
        Task<object> GetAllLineNoOfAdd();
        Task<object> GetAllLineTypeOfAdd(string line_id);
        Task<object> GetAllModelNoOfAdd(string line_id, string line_type_id);
        Task<object> GetAllProdSeasonOfAdd(string line_id, string line_type_id, string model_no);
        Task<List<KeyValuePair<string, string>>> GetAllAttachmentType();

        Task<bool> Create(BL_AttachmentsDTO model);
        Task<bool> DeleteAttachment(BL_AttachmentsDTO model);
        Task<bool> IsExists(BL_AttachmentsDTO model);

    }
}