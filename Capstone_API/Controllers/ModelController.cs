using Microsoft.AspNetCore.Mvc;
using UTA.T2.MusicLibrary.Service.Results;

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
