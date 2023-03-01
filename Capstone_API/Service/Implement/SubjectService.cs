using AutoMapper;
using Capstone_API.Models;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class SubjectService : ISubjectService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<Subject> TestFuntion()
        {
            Subject subject = new Subject();
            subject.Name = "Test";
            subject.Id = 1;
            List<Subject> subjects = new()
            {
               subject
            };

            return subjects;
        }
    }
}
