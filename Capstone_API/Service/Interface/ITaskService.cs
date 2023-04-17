using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITaskService
    {
        ResponseResult TimeTableModify(TaskModifyDTO request);
        GenericResult<SearchDTO> SearchTask(GetAllTaskAssignRequest request);
        GenericResult<TimeSlotInfoResponse> GetAllTaskNotAssign(GetAllRequest request);
        GenericResult<QueryDataByLecturerAndTimeSlot> GetATask(GetATaskDTO request);
        ResponseResult LockAndUnLockTask(LockAndUnLockTaskDTO request);
        ResponseResult UnLockAllTask();
        GenericResult<List<ResponseTaskByLecturerIsKey>> GetTaskAssigned(GetAllRequest request);
        List<ResponseTaskByLecturerIsKey> GetTaskResponseByLecturerKey(GetAllRequest request);
    }
}
