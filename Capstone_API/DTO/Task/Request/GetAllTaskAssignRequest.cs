namespace Capstone_API.DTO.Task.Request
{
    public class GetAllTaskAssignRequest
    {
        public int DepartmentHeadId { get; set; }
        public int SemesterId { get; set; }
        public List<int> ClassIds { get; set; }
        public List<int> LecturerIds { get; set; }
        public List<int> SubjectIds { get; set; }
        public List<int> RoomId { get; set; }

    }
}
