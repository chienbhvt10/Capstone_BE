using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITaskService
    {
        GenericResult<TaskAssignModifyResponse> TimeTableModify(TaskModifyDTO request);
        GenericResult<SearchDTO> SearchTask(DTO.Task.Request.GetAllTaskAssignDTO request);
        GenericResult<TimeSlotInfoResponse> GetAllTaskNotAssign();
        Task<GenericResult<List<ResponseTaskByLecturerIsKey>>> GetSchedule(string executeId);
        GenericResult<QueryDataByLecturerAndTimeSlot> GetATask(int TaskId);
        Task<GenericResult<ExecuteResponse>> Execute(SettingRequest request);
        ResponseResult LockAndUnLockTask(LockAndUnLockTaskDTO request);
        ResponseResult UnLockAllTask();


    }
}
