using Capstone_API.DTO.Class.Response;
using Capstone_API.DTO.CommonRequest;
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

        [HttpPost]
        public GenericResult<IEnumerable<ClassResponse>> Get([FromBody] GetAllRequest request)
        {
            return _classService.GetAll(request);
        }

    }
}
