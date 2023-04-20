using AutoMapper;
using Capstone_API.DTO.Class.Response;
using Capstone_API.DTO.CommonRequest;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ClassService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public GenericResult<IEnumerable<ClassResponse>> GetAll(GetAllRequest request)
        {
            try
            {
                var classes = _unitOfWork.ClassRepository.GetAll()
                .Where(item =>
                    item.SemesterId == request.SemesterId
                    && item.DepartmentHeadId == request.DepartmentHeadId);
                var classesViewModel = _mapper.Map<IEnumerable<ClassResponse>>(classes);
                return new GenericResult<IEnumerable<ClassResponse>>(classesViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<IEnumerable<ClassResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
