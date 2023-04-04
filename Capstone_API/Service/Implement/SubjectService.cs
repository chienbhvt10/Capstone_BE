using AutoMapper;
using Capstone_API.DTO.Subject.Request;
using Capstone_API.DTO.Subject.Response;
using Capstone_API.Models;
using Capstone_API.Results;
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

        public GenericResult<IEnumerable<SubjectResponse>> GetAll()
        {
            try
            {
                var subjects = _unitOfWork.SubjectRepository.GetAll();
                var subjectsViewModel = _mapper.Map<IEnumerable<SubjectResponse>>(subjects);
                return new GenericResult<IEnumerable<SubjectResponse>>(subjectsViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<IEnumerable<SubjectResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public GenericResult<SubjectResponse> GetOneSubject(int Id)
        {
            try
            {
                var subject = _unitOfWork.SubjectRepository.GetById(Id);
                var subjectViewModel = _mapper.Map<SubjectResponse>(subject);

                return new GenericResult<SubjectResponse>(subjectViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<SubjectResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public GenericResult<SubjectResponse> CreateSubject(SubjectRequest request)
        {
            try
            {
                var subject = _mapper.Map<Subject>(request);
                _unitOfWork.SubjectRepository.Add(subject);
                _unitOfWork.Complete();
                var subjectRes = _mapper.Map<SubjectResponse>(subject);

                return new GenericResult<SubjectResponse>(subjectRes, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<SubjectResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult UpdateSubject(SubjectResponse request)
        {
            try
            {
                var subject = _mapper.Map<Subject>(request);
                _unitOfWork.SubjectRepository.Update(subject);
                _unitOfWork.Complete();
                return new ResponseResult("Update successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult DeleteSubject(int id)
        {
            try
            {
                var subject = _unitOfWork.SubjectRepository.Find(id) ?? throw new ArgumentException("Subject does not exist");
                _unitOfWork.SubjectRepository.Delete(subject, isHardDeleted: true);
                _unitOfWork.Complete();
                return new ResponseResult("Delete successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
