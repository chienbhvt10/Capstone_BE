using AutoMapper;
using Capstone_API.DTO.Semester.Request;
using Capstone_API.DTO.Semester.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SemesterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public GenericResult<List<SemesterResponse>> GetAll()
        {
            try
            {
                var semester = _unitOfWork.SemesterRepository.GetAll();
                var semestersViewModel = _mapper.Map<List<SemesterResponse>>(semester);
                return new GenericResult<List<SemesterResponse>>(semestersViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<SemesterResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public GenericResult<SemesterResponse> GetOneSemester(int Id)
        {
            try
            {
                var semester = _unitOfWork.SemesterRepository.GetById(Id);
                var semesterViewModel = _mapper.Map<SemesterResponse>(semester);

                return new GenericResult<SemesterResponse>(semesterViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<SemesterResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public GenericResult<SemesterResponse> CreateSemester(SemesterRequest request)
        {
            try
            {
                var semester = _mapper.Map<Semester>(request);
                _unitOfWork.SemesterRepository.Add(semester);
                _unitOfWork.Complete();

                var semesterRes = _mapper.Map<SemesterResponse>(semester);

                return new GenericResult<SemesterResponse>(semesterRes, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<SemesterResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult UpdateSemester(SemesterResponse request)
        {
            try
            {
                var semester = _mapper.Map<Semester>(request);
                _unitOfWork.SemesterRepository.Update(semester);
                _unitOfWork.Complete();
                return new ResponseResult("Update successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public ResponseResult DeleteSemester(int id)
        {
            try
            {
                var taskContainThisDeleteSemester = _unitOfWork.TaskRepository.GetAll().Where(item => item.SemesterId == id).ToList();

                foreach (var item in taskContainThisDeleteSemester)
                {
                    item.SemesterId = null;
                    _unitOfWork.TaskRepository.Update(item);
                    _unitOfWork.Complete();
                }

                var semester = _unitOfWork.SemesterRepository.Find(id) ?? throw new ArgumentException("Semester does not exist");
                _unitOfWork.SemesterRepository.Delete(semester, isHardDeleted: true);
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
