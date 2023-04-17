namespace Capstone_API.DTO.Subject.Request
{
    public class SubjectRequest
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int SemesterId { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
