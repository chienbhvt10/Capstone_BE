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
                    (item.Username != null && item.Username.Trim().Equals(request.Username))
                    && (item.Password != null && item.Password.Trim().Equals(request.Password))).FirstOrDefault();

                if (userLogin == null)
                {
                    return new GenericResult<LoginResponse>("Username or password wrong");
                }
                LoginResponse response = new()
                {
                    Id = userLogin.Id,
                    Username = userLogin.Username,
                    Department = _unitOfWork.DepartmentRepository.GetByCondition(item => item.Id == userLogin.DepartmentId).FirstOrDefault()?.Department1
                };
                return new GenericResult<LoginResponse>(response, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<LoginResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
