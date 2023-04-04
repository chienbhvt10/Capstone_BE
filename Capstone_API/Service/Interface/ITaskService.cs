using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITaskService
    {
        ResponseResult TimeTableModify(TaskModifyDTO request);
        ResponseResult SwapLecturer(SwapLecturerDTO request);
        ResponseResult SwapRoom(SwapRoomDTO request);
        ResponseResult RequestLecturerConfirm();
        GenericResult<SearchDTO> SearchTask(DTO.Task.Request.GetAllTaskAssignDTO request);
        GenericResult<TimeSlotInfoResponse> GetAllTaskNotAssign();
        Task<GenericResult<List<ResponseTaskByLecturerIsKey>>> GetSchedule(int executeId);
        GenericResult<QueryDataByLecturerAndTimeSlot> GetATask(int TaskId);
        Task<GenericResult<ExecuteResponse>> Execute(SettingRequest request);
        ResponseResult LockAndUnLockTask(LockAndUnLockTaskDTO request);
        ResponseResult UnLockAllTask();


    }
}
