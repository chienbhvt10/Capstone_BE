using AutoMapper;
using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.Lecturer.Request;
using Capstone_API.DTO.Lecturer.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class LecturerService : ILecturerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public LecturerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<List<LecturerResponse>> GetAll(GetLecturerDTO request)
        {
            try
            {
                var lecturers = _unitOfWork.LecturerRepository.MappingLecturerData().Where(item => item.SemesterId == request.SemesterId);
                if (request.TimeSlotId == null && request.SubjectId == null)
                {
                    var lecturersViewModel = _mapper.Map<List<LecturerResponse>>(lecturers);
                    return new GenericResult<List<LecturerResponse>>(lecturersViewModel, true);
                }

                if (request.LecturerId == null)
                {
                    lecturers = lecturers.Where(item => item.Id != request.LecturerId);
                }

                if (request.SubjectId != null)
                {
                    lecturers = lecturers
                        .Where(item => item.SubjectPreferenceLevels
                            .Any(item => item.SubjectId == request.SubjectId && item.PreferenceLevel > 0)).ToList();
                }

                if (request.TimeSlotId != null)
                {
                    // Chua nghi ra

                }

                var lecturersSearchingViewModel = _mapper.Map<List<LecturerResponse>>(lecturers);
                return new GenericResult<List<LecturerResponse>>(lecturersSearchingViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<List<LecturerResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public GenericResult<LecturerResponse> GetOneLecturer(int Id)
        {
            try
            {
                var lecturer = _unitOfWork.LecturerRepository.GetById(Id);
                var lecturerViewModel = _mapper.Map<LecturerResponse>(lecturer);

                return new GenericResult<LecturerResponse>(lecturerViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<LecturerResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #region Create Lecturer
        public void CreateSlotPreferenceForNewLecturer(Lecturer lecturer)
        {
            List<SlotPreferenceLevel> slotPreferenceLevels = new();
            foreach (var item in _unitOfWork.TimeSlotRepository.GetAll())
            {
                slotPreferenceLevels.Add(new SlotPreferenceLevel()
                {
                    SlotId = item.Id,
                    LecturerId = lecturer.Id,
                    PreferenceLevel = 5
                });
            }
            _unitOfWork.SlotPreferenceLevelRepository.AddRange(slotPreferenceLevels);
            _unitOfWork.Complete();
        }

        public void CreateSubjectPreferenceForNewLecturer(Lecturer lecturer)
        {
            List<SubjectPreferenceLevel> slotPreferenceLevels = new();
            foreach (var item in _unitOfWork.SubjectRepository.GetAll())
            {
                slotPreferenceLevels.Add(new SubjectPreferenceLevel()
                {
                    SubjectId = item.Id,
                    LecturerId = lecturer.Id,
                    PreferenceLevel = 0
                });
            }
            _unitOfWork.SubjectPreferenceLevelRepository.AddRange(slotPreferenceLevels);
            _unitOfWork.Complete();
        }

        // need create subject preferencelevel, slot preference level
        public GenericResult<LecturerResponse> CreateLecturer(LecturerRequest request)
        {
            try
            {
                var lecturer = _mapper.Map<Lecturer>(request);
                _unitOfWork.LecturerRepository.Add(lecturer);
                _unitOfWork.Complete();
                var lecturerRes = _mapper.Map<LecturerResponse>(lecturer);

                CreateSubjectPreferenceForNewLecturer(lecturer);
                CreateSlotPreferenceForNewLecturer(lecturer);
                return new GenericResult<LecturerResponse>(lecturerRes, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<LecturerResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion
        public ResponseResult UpdateLecturer(LecturerResponse request)
        {
            try
            {
                var lecturer = _mapper.Map<Lecturer>(request);
                _unitOfWork.LecturerRepository.Update(lecturer);
                _unitOfWork.Complete();
                return new ResponseResult("Update successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        #region DeleteLecturer
        // need delete subject preferencelevel, slot preference level first, and in task assign
        public ResponseResult DeleteLecturer(int id)
        {
            try
            {
                _unitOfWork.SubjectPreferenceLevelRepository.DeleteByCondition(item => item.LecturerId == id, true);
                _unitOfWork.SlotPreferenceLevelRepository.DeleteByCondition(item => item.LecturerId == id, true);

                var taskContainThisDeleteLecturer = _unitOfWork.TaskRepository.GetAll().Where(item => item.LecturerId == id).ToList();

                foreach (var item in taskContainThisDeleteLecturer)
                {
                    item.LecturerId = null;
                    _unitOfWork.TaskRepository.Update(item);
                    _unitOfWork.Complete();
                }

                var lecturer = _unitOfWork.LecturerRepository.Find(id) ?? throw new ArgumentException("Lecturer does not exist");
                _unitOfWork.LecturerRepository.Delete(lecturer, isHardDeleted: true);
                _unitOfWork.Complete();
                return new ResponseResult("Delete successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        #endregion

        public ResponseResult ReUseDataFromASemester(ReUseRequest request)
        {
            try
            {

                var fromLecturerData = _unitOfWork.LecturerRepository.GetAll().Where(item => item.SemesterId == request.FromSemesterId);
                List<Lecturer> newLecturer = new();

                foreach (var item in fromLecturerData)
                {
                    newLecturer.Add(new Lecturer()
                    {
                        DepartmentId = item.DepartmentId,
                        Email = item.Email,
                        MinQuota = item.MinQuota,
                        Quota = item.Quota,
                        ShortName = item.ShortName,
                        Name = item.Name,
                        SemesterId = request.ToSemesterId
                    });
                }
                _unitOfWork.LecturerRepository.AddRange(newLecturer);
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
