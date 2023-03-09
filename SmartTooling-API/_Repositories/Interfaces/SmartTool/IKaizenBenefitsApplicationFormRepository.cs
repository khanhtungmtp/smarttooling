using System.Threading.Tasks;
using SmartTooling_API.Models.SmartTool;
using SmartTooling_API.DTO.SmartTool;
namespace SmartTooling_API._Repositories.Interfaces.SmartTool
{
    public interface IKaizenBenefitsApplicationFormRepository : IMainRepository<Kaizen_Benefits_Application_Form>
    {
        Task<bool> CheckKaizenDescriptionExist(Kaizen_Benefits_Application_FormDTO model, string factory);
    }
}