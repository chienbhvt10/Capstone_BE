namespace Capstone_API.DTO.Semester.Response
{
    public class SemesterResponse
    {
        public int Id { get; set; }
        public bool? IsNow { get; set; }
        public int Year { get; set; }
        public string? Semester { get; set; }
    }
}
