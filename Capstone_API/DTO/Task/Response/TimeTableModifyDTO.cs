namespace Capstone_API.DTO.Task.Response
{
    public class TaskAssignModifyDTO
    {
        public int TaskId { get; set; }
        public int? ClassId { get; set; }
        public string? ClassName { get; set; }
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public int? TimeSlotId { get; set; }
        public string? TimeSlotName { get; set; }
        public bool? Status { get; set; }
        public int? LecturerId { get; set; }
        public string? LecturerName { get; set; }
        public bool? PreAssign { get; set; }
        public int? RoomId { get; set; }
        public string? RoomName { get; set; }
    }
    public class TaskAssignModifyResponse
    {
        public TaskAssignModifyDTO? TaskNeedAssign { get; set; }
        public TaskAssignModifyDTO? TaskSameTimeSlot { get; set; }
    }
}
