using Microsoft.Extensions.Configuration;
using SmartTooling_API._Repositories.Interfaces.Auth;
using SmartTooling_API._Repositories.Repositories.SmartTool;
using SmartTooling_API.Data;
using SmartTooling_API.Models.Auth;

namespace SmartTooling_API._Repositories.Repositories.Auth
{
    public class UserRepository : MainRepository<Users>, IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context, CBDataContext CBcontext, SHCDataContext SHCcontext,
                            SPCDataContext SPCcontext, TSHDataContext TSHcontext, IConfiguration configuration) :
                            base(context, CBcontext, SHCcontext, SPCcontext, TSHcontext, configuration)
        {
            _context = context;
        }
    }
}