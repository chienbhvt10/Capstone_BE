using AutoMapper;
using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.PreferenceLevel.Response;
using Capstone_API.Models;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class SubjectPreferenceLevelService : ISubjectPreferenceLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SubjectPreferenceLevelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<List<GetSubjectPreferenceLevelDTO>> GetAll(GetPreferenceRequest request)
        {
            try
            {
                var query = SubjectPreferenceLevelByLecturerIsKey(request.SemesterId);
                var subjectsViewModel = _mapper.Map<IEnumerable<GetSubjectPreferenceLevelDTO>>(query).ToList();

                return new GenericResult<List<GetSubjectPreferenceLevelDTO>>(subjectsViewModel, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<List<GetSubjectPreferenceLevelDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public IEnumerable<GetSubjectPreferenceLevelDTO> SubjectPreferenceLevelByLecturerIsKey(int semesterId)
        {
            var data = _unitOfWork.SubjectPreferenceLevelRepository.MappingSubjectPreferenceData()
                .Where(item => item.SemesterId == semesterId)
                .OrderBy(item => item.LecturerId).GroupBy(item => item.LecturerId);
            var result = data.Select(group =>
                new GetSubjectPreferenceLevelDTO
                {
                    LecturerId = group.Key ?? 0,
                    SemesterId = group.First().SemesterId ?? 0,
                    LecturerName = group.First().Lecturer?.ShortName ?? "",
                    PreferenceInfos = group.OrderBy(item => item.SubjectId).Select(data =>
                        new SubjectPreferenceInfo
                        {
                            PreferenceId = data.Id,
                            PreferenceLevel = data.PreferenceLevel ?? 0,
                            SubjectId = data.SubjectId ?? 0
                        }).ToList(),
                }).ToList();
            return result;
        }

        public ResponseResult UpdateSubjectPreferenceLevel(UpdateSubjectPreferenceLevelDTO request)
        {
            try
            {
                var subjectPreferenceLevel = _unitOfWork.SubjectPreferenceLevelRepository.Find(item => item.Id == request.PreferenceId);
                subjectPreferenceLevel.PreferenceLevel = request.PreferenceLevel;
                _unitOfWork.SubjectPreferenceLevelRepository.Update(subjectPreferenceLevel);
                _unitOfWork.Complete();
                return new ResponseResult("Update successfully", true);
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
                var currentSemesterLecturer = _unitOfWork.LecturerRepository.GetAll().Where(item => item.SemesterId == request.ToSemesterId).ToList();
                if (currentSemesterLecturer.Count == 0)
                {
                    return new ResponseResult("Reuse fail, this semester have nodata of lecturers, must be reuse of lecturers first", false);
                }

                var currentSemesterSubject = _unitOfWork.SubjectRepository.GetAll().Where(item => item.SemesterId == request.ToSemesterId).ToList();
                if (currentSemesterSubject.Count == 0)
                {
                    return new ResponseResult("Reuse fail, this semester have nodata of subjects, must be reuse of subjects first", false);
                }

                var fromSubjectPreferenceLevelData = _unitOfWork.SubjectPreferenceLevelRepository.GetAll().Where(item => item.SemesterId == request.FromSemesterId);
                List<SubjectPreferenceLevel> newSubjectPreferenceLevel = new();

                foreach (var item in fromSubjectPreferenceLevelData)
                {
                    newSubjectPreferenceLevel.Add(new SubjectPreferenceLevel()
                    {
                        LecturerId = item.LecturerId,
                        SubjectId = item.SubjectId,
                        PreferenceLevel = item.PreferenceLevel,
                        SemesterId = request.ToSemesterId
                    });
                }
                _unitOfWork.SubjectPreferenceLevelRepository.AddRange(newSubjectPreferenceLevel);
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
