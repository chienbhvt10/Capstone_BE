using Capstone_API.DTO.Auth.Request;
using Capstone_API.DTO.Auth.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IAuthService
    {
        GenericResult<LoginResponse> Login(LoginRequest request);
    }
}
