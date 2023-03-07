using Capstone_API.DTO.Subject.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public GenericResult<IEnumerable<SubjectResponse>> Get()
        {
            return _subjectService.GetAll();
        }

        [HttpGet("{id}")]
        public GenericResult<SubjectResponse> Get(int id)
        {
            return _subjectService.GetOneSubject(id);
        }

        [HttpPost]
        public ResponseResult Post([FromBody] SubjectResponse request)
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
    }
}
