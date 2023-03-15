using Capstone_API.DTO.Class.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/class")]
    [ApiController]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;
        public ClassController(IClassService classService)
        {
            _classService = classService;
        }
        // GET: api/<ClassController>
        [HttpGet]
        public GenericResult<IEnumerable<ClassResponse>> Get()
        {
            return _classService.GetAll();
        }

        // GET api/<ClassController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ClassController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ClassController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ClassController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
