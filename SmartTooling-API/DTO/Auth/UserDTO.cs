using System;

namespace SmartTooling_API.DTO.Auth
{
    public class UserDTO
    {
        public string account { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public bool is_active { get; set; }
        public string update_by { get; set; }
        public DateTime? update_time { get; set; }
        public UserDTO()
        {
            this.update_time = DateTime.Now;
        }
    }
}