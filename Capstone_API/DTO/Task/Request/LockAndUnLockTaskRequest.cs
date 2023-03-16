namespace Capstone_API.DTO.Task.Request
{
    public class LockAndUnLockTaskRequest
    {
        public int TaskId { get; set; }
        public int LecturerId { get; set; }

        public LockAndUnLockTaskRequest()
        {

        }

        public LockAndUnLockTaskRequest(int taskId, int lecturerId)
        {
            TaskId = taskId;
            LecturerId = lecturerId;
        }
    }
}
