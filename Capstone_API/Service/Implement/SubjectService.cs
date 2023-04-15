using AutoMapper;
using Capstone_API.DTO.CommonRequest;
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

        public GenericResult<List<SubjectResponse>> GetAll(GetSubjectRequest request)
        {
            try
            {
                var subjects = _unitOfWork.SubjectRepository.GetAll().Where(item => item.SemesterId == request.SemesterId);
                var subjectsViewModel = _mapper.Map<List<SubjectResponse>>(subjects);
                return new GenericResult<List<SubjectResponse>>(subjectsViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<SubjectResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
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

        #region CreateSubject
        public void CreateSubjectPreferenceForNewSubject(Subject subject)
        {
            List<SubjectPreferenceLevel> slotPreferenceLevels = new();
            foreach (var item in _unitOfWork.LecturerRepository.GetAll())
            {
                slotPreferenceLevels.Add(new SubjectPreferenceLevel()
                {
                    SubjectId = subject.Id,
                    LecturerId = item.Id,
                    PreferenceLevel = 0
                });
            }
            _unitOfWork.SubjectPreferenceLevelRepository.AddRange(slotPreferenceLevels);
            _unitOfWork.Complete();
        }

        // need create subject preference level
        public GenericResult<SubjectResponse> CreateSubject(SubjectRequest request)
        {
            try
            {
                var subject = _mapper.Map<Subject>(request);
                _unitOfWork.SubjectRepository.Add(subject);
                _unitOfWork.Complete();

                CreateSubjectPreferenceForNewSubject(subject);
                var subjectRes = _mapper.Map<SubjectResponse>(subject);

                return new GenericResult<SubjectResponse>(subjectRes, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<SubjectResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion
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

        // need delete subject preference level, task assign
        public ResponseResult DeleteSubject(int id)
        {
            try
            {
                _unitOfWork.SubjectPreferenceLevelRepository.DeleteByCondition(item => item.SubjectId == id, true);
                var taskContainThisDeleteSubject = _unitOfWork.TaskRepository.GetAll().Where(item => item.SubjectId == id).ToList();

                foreach (var item in taskContainThisDeleteSubject)
                {
                    item.SubjectId = null;
                    _unitOfWork.TaskRepository.Update(item);
                    _unitOfWork.Complete();
                }

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

        public ResponseResult ReUseDataFromASemester(ReUseRequest request)
        {
            try
            {

                var fromSubjectData = _unitOfWork.SubjectRepository.GetAll().Where(item => item.SemesterId == request.FromSemesterId);
                List<Subject> newSubjects = new();

                foreach (var item in fromSubjectData)
                {
                    newSubjects.Add(new Subject()
                    {
                        Code = item.Code,
                        Name = item.Name,
                        SemesterId = request.ToSemesterId
                    });
                }
                _unitOfWork.SubjectRepository.AddRange(newSubjects);
                _unitOfWork.Complete();

                return new ResponseResult("Reuse data successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
