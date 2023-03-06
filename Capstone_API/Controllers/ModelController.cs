using Capstone_API.Results;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_API.Controllers
{
    [Route("/api/model")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        [HttpGet]
        public ResponseResult Get()
        {
            return new ResponseResult();
        }
    }
}
