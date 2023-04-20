namespace Capstone_API.DTO.Task.Request
{
    public class LockAndUnLockTaskDTO
    {
        public int TaskId { get; set; }
        public int LecturerId { get; set; }

        public LockAndUnLockTaskDTO()
        {

        }

        public LockAndUnLockTaskDTO(int taskId, int lecturerId)
        {
            TaskId = taskId;
            LecturerId = lecturerId;
        }
    }
}
