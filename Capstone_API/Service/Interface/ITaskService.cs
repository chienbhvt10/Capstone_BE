﻿using Capstone_API.DTO.Task.Request;
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
        GenericResult<List<ExecuteResponse>> GetAll(GetAllTaskAssignRequest request);
        Task<GenericResult<List<ExecuteResponse>>> GetSchedule(int executeId);

    }
}
