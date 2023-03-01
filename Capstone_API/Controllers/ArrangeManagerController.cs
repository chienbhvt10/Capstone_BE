using Capstone_API.DTO.Task;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using UTA.T2.MusicLibrary.Service.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/arrange")]
    [ApiController]
    public class ArrangeManagerController : ControllerBase
    {

        private readonly ITaskService _taskService;
        public ArrangeManagerController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPut("swap-lecturer")]
        public ResponseResult SwapLectuter([FromBody] SwapLecturerOfTaskDTO value)
        {
            return _taskService.SwapLecturer(value);
        }

        [HttpPut("swap-room")]
        public ResponseResult SwapRoom([FromBody] SwapRoomOfTaskDTO value)
        {
            return _taskService.SwapRoom(value);
        }
        [HttpPut("timetable-modify")]
        public ResponseResult TimeTableModify([FromBody] TaskModifyDTO value)
        {
            return _taskService.TimeTableModify(value);
        }
    }
}
