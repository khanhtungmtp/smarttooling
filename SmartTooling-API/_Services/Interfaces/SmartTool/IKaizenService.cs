using System.Threading.Tasks;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Interfaces.SmartTool
{
    public interface IKaizenService : IMainService<KaizenDTO>
    {
        Task<PagedList<KaizenDTO>> Search(PaginationParams param, string model_no, string factory);
        Task<KaizenDTO> GetbyID(string modelNO);
        Task<object> GetModelNo(string factory);
        Task<object> GetKaizenFrom();
        Task<object> GetStage(string factory);
        Task<object> Getprocess(string modelNO, string stage, string factory);
        Task<object> Getopera(string modelNO, string stage, string process, string factory);
        Task<KaizenDTO> GetKaizenEdit(string modelNO, string serialNo, string factory);
        Task<OperationResult> AddKaizen(KaizenDTO model);
        Task<OperationResult> UpdateKaizen(KaizenDTO model);
    }
}