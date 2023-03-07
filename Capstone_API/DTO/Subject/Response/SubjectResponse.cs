namespace Capstone_API.DTO.Subject.Response
{
    public class SubjectResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? SemesterId { get; set; }
        public int? OrderNumber { get; set; }
        public string? Department { get; set; }

    }
}
