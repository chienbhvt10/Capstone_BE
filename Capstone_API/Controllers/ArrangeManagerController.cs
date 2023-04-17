using Capstone_API.DTO.CommonRequest;
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
        public GenericResult<SearchDTO> GetTaskAssign([FromBody] GetAllTaskAssignRequest request)
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

        #endregion

        #region Excel Api


        [HttpGet("export-in-import-format")]
        public FileStreamResult ExportInImportFormat()
        {
            return _excelService.ExportInImportFormat(_httpContextAccessor);
        }

        // need import with who user
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

        [HttpPost("export-groupby-lecturer")]
        public FileStreamResult ExportGroupByLecturers([FromBody] GetAllRequest request)
        {
            return _excelService.ExportGroupByLecturers(request, _httpContextAccessor);
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
