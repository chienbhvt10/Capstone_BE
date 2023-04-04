namespace Capstone_API.DTO.Task.Request
{
    public class GetAllTaskAssignDTO
    {
        public GetAllTaskAssignDTO(int semesterId, List<int> classIds, List<int> lecturerIds, List<int> subjectIds, List<int> roomId)
        {
            SemesterId = semesterId;
            ClassIds = classIds;
            LecturerIds = lecturerIds;
            SubjectIds = subjectIds;
            RoomId = roomId;
        }

        public int SemesterId { get; set; }
        public List<int> ClassIds { get; set; }
        public List<int> LecturerIds { get; set; }
        public List<int> SubjectIds { get; set; }
        public List<int> RoomId { get; set; }

        public GetAllTaskAssignDTO()
        {
        }
    }
}
