﻿using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/arrange")]
    [ApiController]
    public class ArrangeManagerController : ControllerBase
    {

        private readonly ITaskService _taskService;
        private readonly IExcelService _excelService;
        private readonly IExecuteInfoService _executeInfoService;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArrangeManagerController(
            ITaskService taskService,
            IExcelService excelService,
            IHttpContextAccessor httpContextAccessor,
            IExecuteInfoService executeInfoService)
        {
            _taskService = taskService;
            _excelService = excelService;
            _httpContextAccessor = httpContextAccessor;
            _executeInfoService = executeInfoService;
        }

        #region Task Api
        [HttpPost("get-a-task")]
        public GenericResult<QueryDataByLecturerAndTimeSlot> GetATask([FromBody] GetATaskDTO request)
        {
            return _taskService.GetATask(request);
        }

        [HttpPost("search-tasks")]
        public GenericResult<SearchDTO> SearchTask([FromBody] GetAllTaskAssignRequest request)
        {
            return _taskService.SearchTask(request);
        }

        [HttpPost("get-tasks-not-assigned")]
        public GenericResult<TimeSlotInfoResponse> GetTaskNotAssigned([FromBody] GetAllRequest request)
        {
            return _taskService.GetAllTaskNotAssign(request);
        }

        [HttpPost("get-tasks-assigned")]
        public GenericResult<List<ResponseTaskByLecturerIsKey>> GetTaskAssigned([FromBody] GetAllRequest request)
        {
            return _taskService.GetTaskAssigned(request);
        }

        [HttpPut("lock-and-unlock-task")]
        public ResponseResult LockAndUnLockTask([FromBody] LockAndUnLockTaskDTO value)
        {
            return _taskService.LockAndUnLockTask(value);
        }

        [HttpPut("unlock-all-task")]
        public ResponseResult UnLockAllTask()
        {
            return _taskService.UnLockAllTask();
        }

        [HttpPut("timetable-modify")]
        public ResponseResult TimeTableModify([FromBody] TaskModifyDTO value)
        {
            return _taskService.TimeTableModify(value);
        }

        [HttpPut("swap-lecturer")]
        public ResponseResult SwapLecturer([FromBody] SwapLecturerDTO request)
        {
            return _taskService.SwapLecturer(request);
        }

        [HttpPut("swap-room")]
        public ResponseResult SwapRoom([FromBody] SwapRoomDTO request)
        {
            return _taskService.SwapRoom(request);
        }

        #endregion

        #region Excel Api

        [HttpPost("import-time-table-result")]
        public async Task<ResponseResult> ImportTimeTableResult([FromForm] IFormFile file, [FromForm] int semesterId, [FromForm] int departmentHeadId, CancellationToken cancellationToken)
        {
            var request = new GetAllRequest()
            {
                DepartmentHeadId = departmentHeadId,
                SemesterId = semesterId
            };
            return await _excelService.ImportTimetableResult(request, file, cancellationToken);
        }


        [HttpGet("export-in-import-format/{userId}")]
        public FileStreamResult ExportInImportFormat(int userId)
        {
            return _excelService.ExportInImportFormat(userId, _httpContextAccessor);
        }

        [HttpPost("import-time-table")]
        public async Task<ResponseResult> ImportTimeTable([FromForm] IFormFile file, [FromForm] int semesterId, [FromForm] int departmentHeadId, CancellationToken cancellationToken)
        {
            var request = new GetAllRequest()
            {
                DepartmentHeadId = departmentHeadId,
                SemesterId = semesterId
            };
            return await _excelService.ImportTimetable(request, file, cancellationToken);
        }

        [HttpGet("export-groupby-lecturer/{userId}")]
        public FileStreamResult ExportGroupByLecturers(int userId)
        {
            return _excelService.ExportGroupByLecturers(userId, _httpContextAccessor);
        }
        #endregion

        #region Schedule Api

        [HttpGet("get-schedule/{executeId}")]
        public async Task<ResponseResult> GetSchedule(string executeId)
        {
            return await _executeInfoService.GetSchedule(executeId);
        }

        [HttpPost("execute-info/get")]
        public GenericResult<List<ExecuteInfoResponse>> GetExecuteInfo([FromBody] GetAllRequest request)
        {
            return _executeInfoService.GetAll(request);
        }

        [HttpPost("execute-info")]
        public ResponseResult CreateExecuteInfo([FromBody] CreateExecuteInfoRequest request)
        {
            return _executeInfoService.CreateExecuteInfo(request);
        }

        [HttpPost("execute")]
        public async Task<GenericResult<ExecuteResponse>> Execute(SettingRequest request)
        {
            return await _executeInfoService.Execute(request);
        }
        #endregion

    }
}
