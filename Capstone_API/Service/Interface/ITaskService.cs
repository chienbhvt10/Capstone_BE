using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITaskService
    {
        ResponseResult TimeTableModify(TaskModifyDTO request);
        GenericResult<SearchDTO> SearchTask(DTO.Task.Request.GetAllTaskAssignDTO request);
        GenericResult<TimeSlotInfoResponse> GetAllTaskNotAssign();
        GenericResult<QueryDataByLecturerAndTimeSlot> GetATask(int TaskId);
        ResponseResult LockAndUnLockTask(LockAndUnLockTaskDTO request);
        ResponseResult UnLockAllTask();
        GenericResult<List<ResponseTaskByLecturerIsKey>> GetTaskAssigned();
        List<ResponseTaskByLecturerIsKey> GetTaskResponseByLecturerKey();
    }
}
