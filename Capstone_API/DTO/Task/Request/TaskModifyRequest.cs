namespace Capstone_API.DTO.Task.Request
{
    public class TaskModifyRequest
    {
        public int TaskId { get; set; }
        public int TimeSlotId { get; set; }
        public int LecturerId { get; set; }
        public int RoomId { get; set; }


        public TaskModifyRequest()
        {

        }

        public TaskModifyRequest(int taskId, int timeSlotId, int lecturerId, int roomId)
        {
            TaskId = taskId;
            TimeSlotId = timeSlotId;
            LecturerId = lecturerId;
            RoomId = roomId;
        }
    }
}
