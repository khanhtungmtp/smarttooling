using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.DTO.SmartTool;
using SmartTooling_API.Models.SmartTool;

namespace SmartTooling_API._Repositories.Repositories.SmartTool
{
    public class KaizenBenefitsApplicationFormRepository : MainRepository<Kaizen_Benefits_Application_Form>, IKaizenBenefitsApplicationFormRepository
    {
        private readonly SHCDataContext _SHCcontext;
        private readonly CBDataContext _CBcontext;
        private readonly TSHDataContext _TSHcontext;
        private readonly SPCDataContext _SPCcontext;
        public KaizenBenefitsApplicationFormRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext, SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) : base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _SHCcontext = SHCcontext;
            _CBcontext = CBcontext;
            _TSHcontext = TSHcontext;
            _SPCcontext = SPCcontext;
        }

        public async Task<bool> CheckKaizenDescriptionExist(Kaizen_Benefits_Application_FormDTO model, string factory)
        {
            if (factory.Trim() == "SHC" && await _SHCcontext.Kaizen_Benefits_Application_Form.AnyAsync(x => x.factory_id == model.factory_id &&
                                                        x.model_no == model.model_no &&
                                                        x.serial_no == model.serial_no &&
                                                        x.to_factory_id == model.to_factory_id))
            {
                return true;
            }
            if (factory.Trim() == "CB" && await _CBcontext.Kaizen_Benefits_Application_Form.AnyAsync(x => x.factory_id == model.factory_id &&
                                                     x.model_no == model.model_no &&
                                                     x.serial_no == model.serial_no &&
                                                     x.to_factory_id == model.to_factory_id))
            {
                return true;
            }
            if (factory.Trim() == "TSH" && await _SHCcontext.Kaizen_Benefits_Application_Form.AnyAsync(x => x.factory_id == model.factory_id &&
                                                     x.model_no == model.model_no &&
                                                     x.serial_no == model.serial_no &&
                                                     x.to_factory_id == model.to_factory_id))
            {
                return true;
            }
            if (factory.Trim() == "SPC" && await _SHCcontext.Kaizen_Benefits_Application_Form.AnyAsync(x => x.factory_id == model.factory_id &&
                                                     x.model_no == model.model_no &&
                                                     x.serial_no == model.serial_no &&
                                                     x.to_factory_id == model.to_factory_id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}