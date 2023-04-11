namespace Capstone_API.DTO.Task.Request
{
    public class TaskModifyDTO
    {
        public int SubjectId { get; set; }
        public int TaskId { get; set; }
        public int TimeSlotId { get; set; }
        public int LecturerId { get; set; }
        public int RoomId { get; set; }
    }
}
