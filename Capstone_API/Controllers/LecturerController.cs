using Capstone_API.DTO.Lecturer.Request;
using Capstone_API.DTO.Lecturer.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/lecturer")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;
        public LecturerController(ILecturerService lecturerService)
        {
            _lecturerService = lecturerService;
        }

        [HttpGet]
        public GenericResult<List<LecturerResponse>> Get([FromBody] GetLecturerDTO request)
        {
            return _lecturerService.GetAll(request);
        }

        [HttpGet("{id}")]
        public GenericResult<LecturerResponse> Get(int id)
        {
            return _lecturerService.GetOneLecturer(id);
        }

        [HttpPost]
        public GenericResult<LecturerResponse> Post([FromBody] LecturerRequest request)
        {
            return _lecturerService.CreateLecturer(request);
        }

        [HttpPut]
        public ResponseResult Put([FromBody] LecturerResponse request)
        {
            return _lecturerService.UpdateLecturer(request);

        }

        [HttpDelete("{id}")]
        public ResponseResult Delete(int id)
        {
            return _lecturerService.DeleteLecturer(id);

        }
    }
}
