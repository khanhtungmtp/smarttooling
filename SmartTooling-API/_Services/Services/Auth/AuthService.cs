using System.Linq;
using System.Threading.Tasks;
using SmartTooling_API._Repositories.Interfaces.Auth;
using SmartTooling_API._Services.Interfaces.Auth;
using Microsoft.EntityFrameworkCore;
using SmartTooling_API.DTO.Auth;

namespace SmartTooling_API._Services.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _repoUsers;
        private readonly IRoleRepository _repoRoles;
        private readonly IRoleUserRepository _repoRoleUser;
        public AuthService(IUserRepository repoUsers,
                            IRoleRepository repoRoles,
                            IRoleUserRepository repoRoleUser)
        {
            _repoRoleUser = repoRoleUser;
            _repoRoles = repoRoles;
            _repoUsers = repoUsers;
        }

        public async Task<UserForLoggedDTO> GetUser(string username, string password)
        {
            var user = _repoUsers.FindSingle(x => x.account.Trim() == username.Trim() && x.is_active == true);

            // kiểm tra xem username đó có ko
            if (user == null)
            {
                return null;
            }
            if (user.password != password)
            {
                return null;
            }
            var roleUser = _repoRoleUser.FindAll(x => x.user_account == user.account);
            var role = _repoRoles.FindAll();
            var roleName = await roleUser.Join(role, x => x.role_unique, y => y.role_unique, (x, y)
            => new RoleDTO { Name = y.role_unique, Position = y.role_sequence }).ToListAsync();

            var result = new UserForLoggedDTO
            {
                Id = user.account,
                Email = user.email,
                Username = user.account,
                Name = user.name,
                // Nik = user.,
                Role = roleName.OrderBy(x => x.Position).Select(x => x.Name).ToList()
            };

            return result;
        }
    }
}