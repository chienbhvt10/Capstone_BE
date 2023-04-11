using Capstone_API.DTO.Task.Fetch;
using Capstone_API.DTO.Task.Request;
using Capstone_API.DTO.Task.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public GenericResult<SearchDTO> GetTaskAssign([FromBody] DTO.Task.Request.GetAllTaskAssignDTO request)
        {
            return _taskService.SearchTask(request);
        }

        [HttpGet("get-tasks-not-assigned")]
        public GenericResult<TimeSlotInfoResponse> GetTaskNotAssigned()
        {
            return _taskService.GetAllTaskNotAssign();
        }

        [HttpGet("get-tasks-assigned")]
        public GenericResult<List<ResponseTaskByLecturerIsKey>> GetTaskAssigned()
        {
            return _taskService.GetTaskAssigned();
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

        [HttpGet]
        [Route("api/export/excel")]
        public HttpResponseMessage ExportToExcel()
        {
            // Create a new workbook and worksheet
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            // Add some data to the worksheet
            worksheet.Cell("A1").Value = "Name";
            worksheet.Cell("B1").Value = "Age";
            worksheet.Cell("A2").Value = "John";
            worksheet.Cell("B2").Value = 25;
            worksheet.Cell("A3").Value = "Jane";
            worksheet.Cell("B3").Value = 30;

            // Save the workbook to a memory stream
            var stream = new MemoryStream();
            workbook.SaveAs(stream);

            // Set the content type and headers for the response
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(stream.ToArray());
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "Export.xlsx";
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return response;
        }

        [HttpGet("export-in-import-format")]
        public FileStreamResult ExportInImportFormat()
        {
            return _excelService.ExportInImportFormat(_httpContextAccessor);
        }

        [HttpPost("import-time-table")]
        public async Task<ResponseResult> ImportTimeTable(IFormFile file, CancellationToken cancellationToken)
        {
            return await _excelService.ImportTimetable(file, cancellationToken);
        }

        [HttpGet("export-groupby-lecturer")]
        public FileStreamResult ExportGroupByLecturers()
        {
            return _excelService.ExportGroupByLecturers(_httpContextAccessor);
        }
        #endregion

        #region Schedule Api

        [HttpGet("get-schedule/{executeId}")]
        public async Task<ResponseResult> GetSchedule(string executeId)
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
        public async Task<GenericResult<ExecuteResponse>> Execute(SettingRequest request)
        {
            return await _taskService.Execute(request);
        }
        #endregion

    }
}
