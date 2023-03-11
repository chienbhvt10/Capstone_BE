namespace Capstone_API.DTO.Task.Response
{
    public class ExecuteResponse
    {
        public int LecturerId { get; set; }
        public string? LecturerName { get; set; }
        public List<TaskOfLecturer>? Tasks { get; set; }
    }
    public class TaskOfLecturer
    {
        public int TaskId { get; set; }
        public SubjectOfTask? Subject { get; set; }
        public TimeSlotOfTask? TimeSlotOfTask { get; set; }
        public RoomOfTask? RoomOfTask { get; set; }
        public ClassOfTask? ClassOfTask { get; set; }

    }
    public class SubjectOfTask
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
    }
    public class TimeSlotOfTask
    {
        public int TimeSlotId { get; set; }
        public string? TimeSlotCode { get; set; }
        public int TimeSlotOrder { get; set; }
    }
    public class RoomOfTask
    {
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
    }
    public class ClassOfTask
    {
        public int ClassId { get; set; }
        public string? ClassName { get; set; }

    }
}
