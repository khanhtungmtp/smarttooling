using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTooling_API.DTO.Auth;
using SmartTooling_API.Helpers;
using SmartTooling_API.Helpers.Params;
using SmartTooling_API.Helpers.Params.SmartTool;

namespace SmartTooling_API._Services.Interfaces.Auth
{
    public interface IUserService
    {
        Task<PagedList<UserDTO>> GetListUserPaging(string account, string isActive, int pageNumber = 10, int pageSize = 10);
        Task<bool> AddUser(UserDTO user, string updateBy);
        Task<bool> UpdateUser(UserDTO user, string updateBy);
        Task<List<RoleByUserDTO>> GetRoleByUser(string account);
        Task<bool> UpdateRoleByUser(string account, List<RoleByUserDTO> roles, string updateBy);
        Task<bool> CheckExistUser(string account);
        Task<OperationResult> ChangePassword(UserForLoginDto user);
    }
}