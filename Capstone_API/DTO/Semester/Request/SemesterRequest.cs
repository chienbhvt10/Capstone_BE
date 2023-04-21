namespace Capstone_API.DTO.Semester.Request
{
    public class SemesterRequest
    {
        public bool? IsNow { get; set; }
        public int Year { get; set; }
        public string? Semester { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
