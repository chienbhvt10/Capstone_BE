namespace Capstone_API.DTO.Task.Request
{
    public class GetAllTaskAssignRequest
    {
        public GetAllTaskAssignRequest(int semesterId, IEnumerable<int>? classIds, IEnumerable<int>? lecturerIds, IEnumerable<int>? subjectIds, IEnumerable<int>? roomId)
        {
            SemesterId = semesterId;
            ClassIds = classIds;
            LecturerIds = lecturerIds;
            SubjectIds = subjectIds;
            RoomId = roomId;
        }

        public int SemesterId { get; set; }
        public IEnumerable<int>? ClassIds { get; set; }
        public IEnumerable<int>? LecturerIds { get; set; }
        public IEnumerable<int>? SubjectIds { get; set; }
        public IEnumerable<int>? RoomId { get; set; }

        public GetAllTaskAssignRequest()
        {
        }
    }
}
