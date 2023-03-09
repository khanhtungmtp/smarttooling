using System.Threading.Tasks;
using SmartTooling_API.DTO.Auth;

namespace SmartTooling_API._Services.Interfaces.Auth
{
    public interface IAuthService
    {
        Task<UserForLoggedDTO> GetUser(string username, string password);
    }
}