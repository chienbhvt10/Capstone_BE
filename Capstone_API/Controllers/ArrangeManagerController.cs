﻿using Capstone_API.DTO.Excel;
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
        [HttpGet("get-a-task/{taskId}")]
        public GenericResult<QueryDataByLecturerAndTimeSlot> GetATask(int taskId)
        {
            return _taskService.GetATask(taskId);
        }

        [HttpPost("search-tasks")]
        public GenericResult<List<QueryDataByLecturerAndTimeSlot>> GetTaskAssign([FromBody] GetAllTaskAssignRequest request)
        {
            return _taskService.SearchTask(request);
        }

        [HttpGet("get-tasks-not-assigned")]
        public GenericResult<TimeSlotInfoResponse> GetTaskNotAssign()
        {
            return _taskService.GetAllTaskNotAssign();
        }

        [HttpPut("swap-lecturer")]
        public ResponseResult SwapLectuter([FromBody] SwapLecturerRequest value)
        {
            return _taskService.SwapLecturer(value);
        }

        [HttpPut("lock-and-unlock-task")]
        public ResponseResult LockAndUnLockTask([FromBody] LockAndUnLockTaskRequest value)
        {
            return _taskService.LockAndUnLockTask(value);
        }

        [HttpPut("unlock-all-task")]
        public ResponseResult UnLockAllTask()
        {
            return _taskService.UnLockAllTask();
        }


        [HttpPut("swap-room")]
        public ResponseResult SwapRoom([FromBody] SwapRoomRequest value)
        {
            return _taskService.SwapRoom(value);
        }

        [HttpPut("timetable-modify")]
        public ResponseResult TimeTableModify([FromBody] TaskModifyRequest value)
        {
            return _taskService.TimeTableModify(value);
        }

        #endregion

        #region Excel Api

        [HttpPost("export-in-import-format")]
        public GenericResult<string> ExportInImportFormat([FromBody] IEnumerable<ExportInImportFormatDTO> exportItem)
        {
            return _excelService.ExportInImportFormat(_httpContextAccessor, exportItem);
        }

        [HttpPost("import-time-table")]
        public async Task<ResponseResult> ImportTimeTable(IFormFile file, CancellationToken cancellationToken)
        {
            return await _excelService.ImportTimetable(file, cancellationToken);
        }
        #endregion

        #region Schedule Api

        [HttpGet("get-schedule/{executeId}")]
        public async Task<GenericResult<List<ResponseTaskByLecturerIsKey>>> GetSchedule(int executeId)
        {
            return await _taskService.GetSchedule(executeId);
        }

        [HttpGet("execute-info")]
        public GenericResult<List<ExecuteInfoResponse>> GetExecuteInfo()
        {
            return _executeInfoService.GetAll();
        }

        [HttpPost("execute-info")]
        public ResponseResult CreateExecuteInfo([FromBody] ExecuteInfoResponse request)
        {
            return _executeInfoService.CreateExecuteInfo(request);
        }

        [HttpPost("execute")]
        public async Task<GenericResult<int>> Execute(SettingRequest request)
        {
            return await _taskService.Execute(request);
        }
        #endregion
    }
}
