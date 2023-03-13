namespace Capstone_API.DTO.Task.Response
{
    public class ExecuteResponse2
    {
        public int LecturerId { get; set; }
        public string? LecturerName { get; set; }
        public List<TimeSlotOfTask2>? TimeSlotOfTask2 { get; set; }
    }

    public class TimeSlotOfTask2
    {
        public int TimeSlotId { get; set; }
        public string? TimeSlotCode { get; set; }
        public int TimeSlotOrder { get; set; }
        public int TaskId { get; set; }
        public SubjectOfTask? Subject { get; set; }
        public RoomOfTask? RoomOfTask { get; set; }
        public ClassOfTask? ClassOfTask { get; set; }
    }

    public class SubjectOfTask2
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
    }

    public class RoomOfTask2
    {
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
    }
    public class ClassOfTask2
    {
        public int ClassId { get; set; }
        public string? ClassName { get; set; }

    }
}
