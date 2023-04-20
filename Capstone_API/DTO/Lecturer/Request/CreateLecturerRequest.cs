namespace Capstone_API.DTO.Lecturer.Request
{
    public class CreateLecturerRequest
    {
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Email { get; set; }
        public int? Quota { get; set; }
        public int? MinQuota { get; set; }
        public int SemesterId { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
