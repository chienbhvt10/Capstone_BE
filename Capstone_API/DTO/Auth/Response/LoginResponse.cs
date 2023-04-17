namespace Capstone_API.DTO.Auth.Response
{
    public class LoginResponse
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public int? DepartmentId { get; set; }
    }
}
