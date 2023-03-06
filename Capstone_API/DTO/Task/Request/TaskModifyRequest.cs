namespace Capstone_API.DTO.Task.Request
{
    public class TaskModifyRequest
    {
        public int TaskId { get; set; }
        public int LecturerId { get; set; }

        public TaskModifyRequest()
        {

        }

        public TaskModifyRequest(int taskId, int lecturerId)
        {
            TaskId = taskId;
            LecturerId = lecturerId;
        }
    }
}
