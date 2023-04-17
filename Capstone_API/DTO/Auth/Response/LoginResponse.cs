namespace Capstone_API.DTO.Auth.Response
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Department { get; set; }
    }
}
