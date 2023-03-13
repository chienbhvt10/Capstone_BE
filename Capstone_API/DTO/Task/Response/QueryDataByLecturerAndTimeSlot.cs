namespace Capstone_API.DTO.Task.Response
{
    public class QueryDataByLecturerAndTimeSlot
    {
        public int TaskId { get; set; }
        public int LecturerId { get; set; }
        public string? LecturerName { get; set; }
        public int TimeSlotId { get; set; }
        public string? TimeSlotName { get; set; }
        public int? TimeSlotOrder { get; set; }
        public int ClassId { get; set; }
        public string? ClassName { get; set; }
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public int SemesterId { get; set; }
        public int RoomId { get; set; }
        public string? RoomName { get; set; }
        public string? Status { get; set; }
        public int IsAssign { get; set; }
    }
}
