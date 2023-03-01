namespace Capstone_API.DTO.Task
{
    public class TaskModifyDTO
    {
        public int TaskId { get; set; }
        public int LecturerId { get; set; }

        public TaskModifyDTO()
        {

        }

        public TaskModifyDTO(int taskId, int lecturerId)
        {
            TaskId = taskId;
            LecturerId = lecturerId;
        }
    }
}
