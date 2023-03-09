using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTooling_API.DTO.SmartTool;

namespace SmartTooling_API._Services.Interfaces
{
    public interface ISharedResourcesService
    {
        Task<List<KeyValuePair<string, string>>> GetAllFactory();
        Task<List<KeyValuePair<string, string>>> GetAllLine(string factory);
        Task<List<KeyValuePair<string, string>>> GetAllLineType(string factory);
        Task<List<KeyValuePair<string, string>>> GetStage(string factory);
        Task<List<Model_List_DTO>> GetAllModel();
    }
}