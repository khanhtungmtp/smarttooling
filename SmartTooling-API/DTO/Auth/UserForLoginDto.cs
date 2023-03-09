namespace SmartTooling_API.DTO.Auth
{
    public class UserForLoginDto
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}