using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API._Repositories.Interfaces.Auth;
using SmartTooling_API.Data;

namespace SmartTooling_API._Repositories.Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }


    }
}