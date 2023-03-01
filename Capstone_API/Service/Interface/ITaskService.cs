using Capstone_API.DTO.Task;
using UTA.T2.MusicLibrary.Service.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITaskService
    {
        ResponseResult TimeTableModify(TaskModifyDTO request);
        ResponseResult SwapLecturer(SwapLecturerOfTaskDTO request);
        ResponseResult SwapRoom(SwapRoomOfTaskDTO request);
        ResponseResult RequestLecturerConfirm();

    }
}
