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
        GenericResult<IEnumerable<GetAllTaskAssignResponse>> GetAll(GetAllTaskAssignRequest request);


    }
}
