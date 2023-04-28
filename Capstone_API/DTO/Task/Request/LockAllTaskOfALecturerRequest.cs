namespace Capstone_API.DTO.Task.Request
{
    public class LockAllTaskOfALecturerRequest
    {
        public int LecturerId { get; set; }
        public int SemesterId { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
