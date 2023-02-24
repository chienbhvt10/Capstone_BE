using Capstone_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_API.Controllers
{
    [ApiController]
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet("/")]
        public GenericResult Index()
        {
            return new GenericResult(true, "Demo Get xong nhé em");
        }
    }
}
