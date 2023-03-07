using Capstone_API.DTO.Excel;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArrangeManagerController(ITaskService taskService, IExcelService excelService, IHttpContextAccessor httpContextAccessor)
        {
            _taskService = taskService;
            _excelService = excelService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("get-tasks")]
        public GenericResult<List<GetAllTaskAssignResponse>> GetTaskAssign([FromBody] GetAllTaskAssignRequest request)
        {
            return _taskService.GetAll(request);
        }

        [HttpPut("swap-lecturer")]
        public ResponseResult SwapLectuter([FromBody] SwapLecturerRequest value)
        {
            return _taskService.SwapLecturer(value);
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

        [HttpPost("export-in-import-format")]
        public GenericResult<string> ExportInImportFormat([FromBody] IEnumerable<ExportInImportFormatDTO> exportItem)
        {
            return _excelService.ExportInImportFormat(_httpContextAccessor, exportItem);
        }

        [HttpPost("import-time-table")]
        public async Task<GenericResult<IEnumerable<TaskAssignImportDTO>>> ImportTimeTable(IFormFile file, CancellationToken cancellationToken)
        {
            return await _excelService.ImportTimetable(file, cancellationToken);
        }

    }
}
