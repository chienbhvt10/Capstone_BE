using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITaskService
    {
        ResponseResult TimeTableModify(TaskModifyRequest request);
        ResponseResult SwapLecturer(SwapLecturerRequest request);
        ResponseResult SwapRoom(SwapRoomRequest request);
        ResponseResult RequestLecturerConfirm();
        GenericResult<SearchResponse> SearchTask(GetAllTaskAssignRequest request);
        GenericResult<TimeSlotInfoResponse> GetAllTaskNotAssign();
        Task<GenericResult<List<ResponseTaskByLecturerIsKey>>> GetSchedule(int executeId);
        GenericResult<QueryDataByLecturerAndTimeSlot> GetATask(int TaskId);
        Task<GenericResult<int>> Execute(SettingRequest request);
        ResponseResult LockAndUnLockTask(LockAndUnLockTaskRequest request);
        ResponseResult UnLockAllTask();


    }
}
