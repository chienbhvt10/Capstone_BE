namespace Capstone_API.DTO.Lecturer.Response
{
    public class LecturerResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Email { get; set; }
        public int? Quota { get; set; }
        public int? MinQuota { get; set; }
        public int? SemesterId { get; set; }
    }
}
