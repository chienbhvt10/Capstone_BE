using Capstone_API.DTO.Auth.Request;
using Capstone_API.DTO.Auth.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Capstone_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public GenericResult<LoginResponse> Login([FromBody] LoginRequest request)
        {
            return _authService.Login(request);
        }
    }
}
