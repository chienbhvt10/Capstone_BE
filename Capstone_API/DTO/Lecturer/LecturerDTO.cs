namespace Capstone_API.DTO.Lecturer
{
    public class LecturerDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int? SemesterId { get; set; }
        public int? OrderNumber { get; set; }
    }
}
