using Capstone_API.DTO.Lecturer;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using UTA.T2.MusicLibrary.Service.Results;

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
        public GenericResult<IEnumerable<LecturerDTO>> Get()
        {
            return _lecturerService.GetAll();
        }

        [HttpGet("{id}")]
        public GenericResult<LecturerDTO> Get(int id)
        {
            return _lecturerService.GetOneLecturer(id);
        }

        [HttpPost]
        public ResponseResult Post([FromBody] LecturerDTO request)
        {
            return _lecturerService.CreateLecturer(request);
        }

        [HttpPut]
        public ResponseResult Put([FromBody] LecturerDTO request)
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
