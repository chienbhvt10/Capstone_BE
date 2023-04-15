using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.Subject.Request;
using Capstone_API.DTO.Subject.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost("get")]
        public GenericResult<List<SubjectResponse>> Get([FromBody] GetSubjectRequest request)
        {
            return _subjectService.GetAll(request);
        }

        [HttpGet("{id}")]
        public GenericResult<SubjectResponse> Get(int id)
        {
            return _subjectService.GetOneSubject(id);
        }

        [HttpPost]
        public GenericResult<SubjectResponse> Post([FromBody] SubjectRequest request)
        {
            return _subjectService.CreateSubject(request);
        }

        [HttpPut]
        public ResponseResult Put([FromBody] SubjectResponse request)
        {
            return _subjectService.UpdateSubject(request);

        }

        [HttpDelete("{id}")]
        public ResponseResult Delete(int id)
        {
            return _subjectService.DeleteSubject(id);

        }
        [HttpPost("reuse")]
        public ResponseResult ReUseSlotPreferenceDataFromASemester([FromBody] ReUseRequest request)
        {
            return _subjectService.ReUseDataFromASemester(request);
        }
    }
}
