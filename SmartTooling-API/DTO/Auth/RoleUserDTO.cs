using System;

namespace SmartTooling_API.DTO.Auth
{
    public class RoleUserDTO
    {
        public string user_account { get; set; }
        public string role_unique { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
    }
}