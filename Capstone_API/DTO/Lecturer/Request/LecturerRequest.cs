namespace Capstone_API.DTO.Lecturer.Request
{
    public class LecturerRequest
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Email { get; set; }
        public int? Quota { get; set; }
        public int? MinQuota { get; set; }
    }
}
