namespace Capstone_API.DTO.Task.Response
{
    public class GetTaskReponseBySubjectIsKey
    {
        public int SubjectId { get; set; }
        public string? SubjectCode { get; set; }
        public int Total { get; set; }
        public List<TimeSlotInfo2>? TimeSlotInfos { get; set; }
    }

    public class TimeSlotInfo2
    {
        public int TaskId { get; set; }
        public int TimeSlotId { get; set; }
        public string? TimeSlotName { get; set; }
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
    }
}
