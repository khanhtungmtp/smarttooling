using System.Threading.Tasks;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Repositories.Interfaces.SmartTool
{
    public interface IDefectReasonRepository : IMainRepository<Defect_Reason>
    {
        Task<bool> CheckDefectReasonExists(string defectreasonID);
    }
}