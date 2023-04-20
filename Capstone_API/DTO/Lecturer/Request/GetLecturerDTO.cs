namespace Capstone_API.DTO.Lecturer.Request
{
    public class GetLecturerDTO
    {
        public int? SubjectId { get; set; }
        public int? TimeSlotId { get; set; }
        public int? LecturerId { get; set; }
        public int SemesterId { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
