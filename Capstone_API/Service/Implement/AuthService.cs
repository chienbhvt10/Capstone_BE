using AutoMapper;
using Capstone_API.DTO.Auth.Request;
using Capstone_API.DTO.Auth.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AuthService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var userLogin = _unitOfWork.UserRepository.GetAll()
                    .Where(item =>
                    (item.Username != null && item.Username.Trim().Equals(request.UserName))
                    && (item.Password != null && item.Password.Trim().Equals(request.Password)));

                var response = _mapper.Map<LoginResponse>(userLogin);
                return new GenericResult<LoginResponse>(response, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<LoginResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
