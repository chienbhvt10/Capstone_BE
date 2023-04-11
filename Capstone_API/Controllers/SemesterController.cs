using Capstone_API.DTO.Semester.Request;
using Capstone_API.DTO.Semester.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/semester")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        private readonly ISemesterService _semesterService;
        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        [HttpGet]
        public GenericResult<List<SemesterResponse>> Get()
        {
            return _semesterService.GetAll();
        }

        [HttpGet("{id}")]
        public GenericResult<SemesterResponse> Get(int id)
        {
            return _semesterService.GetOneSemester(id);
        }

        [HttpPost]
        public GenericResult<SemesterResponse> Post([FromBody] SemesterRequest request)
        {
            return _semesterService.CreateSemester(request);
        }

        [HttpPut]
        public ResponseResult Put([FromBody] SemesterResponse request)
        {
            return _semesterService.UpdateSemester(request);

        }

        [HttpDelete("{id}")]
        public ResponseResult Delete(int id)
        {
            return _semesterService.DeleteSemester(id);

        }
    }
}
