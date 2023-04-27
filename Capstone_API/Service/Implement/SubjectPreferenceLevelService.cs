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

        public GenericResult<GetSubjectPreferenceLevelResponse> GetAll(GetSubjectPreferenceLevelrequest request)
        {
            try
            {
                var getAllRequest = new GetAllRequest()
                {
                    DepartmentHeadId = request?.GetAllRequest?.DepartmentHeadId ?? 0,
                    SemesterId = request?.GetAllRequest?.SemesterId ?? 0
                };

                var query = SubjectPreferenceLevelByLecturerIsKey(getAllRequest);


                if (request?.Lecturer != null)
                {
                    query = query.Where(item => item.LecturerName.Contains(request.Lecturer));
                }

                query = query.Skip((request.Pagination.PageNumber - 1) * request.Pagination.PageSize)
                    .Take(request.Pagination.PageSize);

                var subjectsViewModel = _mapper.Map<IEnumerable<GetSubjectPreferenceLevelDTO>>(query).ToList();
                var response = new GetSubjectPreferenceLevelResponse()
                {
                    SubjectPreferenceLevels = subjectsViewModel,
                    Total = SubjectPreferenceLevelByLecturerIsKey(getAllRequest).Count(),
                };

                return new GenericResult<GetSubjectPreferenceLevelResponse>(response, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<GetSubjectPreferenceLevelResponse>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public IEnumerable<GetSubjectPreferenceLevelDTO> SubjectPreferenceLevelByLecturerIsKey(GetAllRequest request)
        {
            var data = _unitOfWork.SubjectPreferenceLevelRepository.MappingSubjectPreferenceData()
                .Where(item => item.SemesterId == request.SemesterId && item.DepartmentHeadId == request.DepartmentHeadId)
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
                var currentSemesterLecturer = _unitOfWork.LecturerRepository
                    .GetAll()
                    .Where(item =>
                        item.SemesterId == request.ToSemesterId
                        && item.DepartmentHeadId == request.DepartmentHeadId)
                    .ToList();
                if (currentSemesterLecturer.Count == 0)
                {
                    return new ResponseResult("Reuse fail, this semester have nodata of lecturers, must be reuse of lecturers first", false);
                }

                var currentSemesterSubject = _unitOfWork.SubjectRepository
                    .GetAll()
                    .Where(item =>
                        item.SemesterId == request.ToSemesterId
                        && item.DepartmentHeadId == request.DepartmentHeadId)
                    .ToList();
                if (currentSemesterSubject.Count == 0)
                {
                    return new ResponseResult("Reuse fail, this semester have nodata of subjects, must be reuse of subjects first", false);
                }

                var fromSubjectPreferenceLevelData = _unitOfWork.SubjectPreferenceLevelRepository
                    .GetAll()
                    .Where(item =>
                        item.SemesterId == request.FromSemesterId
                        && item.DepartmentHeadId == request.DepartmentHeadId).ToList();
                List<SubjectPreferenceLevel> newSubjectPreferenceLevel = new();

                foreach (var item in fromSubjectPreferenceLevelData)
                {
                    var lecturerNameInOldSemester = _unitOfWork.LecturerRepository.GetById(item.LecturerId ?? 0)?.ShortName;
                    var lecturerInCurrentSemester = _unitOfWork.LecturerRepository
                        .GetByCondition(item =>
                            item.SemesterId == request.ToSemesterId
                            && item.DepartmentHeadId == request.DepartmentHeadId
                            && item.ShortName == lecturerNameInOldSemester).FirstOrDefault();

                    var subjectCodeInOldSemester = _unitOfWork.SubjectRepository.GetById(item.SubjectId ?? 0)?.Code;
                    var subjectInCurrentSemester = _unitOfWork.SubjectRepository
                        .GetByCondition(item =>
                            item.SemesterId == request.ToSemesterId
                            && item.DepartmentHeadId == request.DepartmentHeadId
                            && item.Code == subjectCodeInOldSemester).FirstOrDefault();

                    newSubjectPreferenceLevel.Add(new SubjectPreferenceLevel()
                    {
                        LecturerId = lecturerInCurrentSemester?.Id,
                        SubjectId = subjectInCurrentSemester?.Id,
                        PreferenceLevel = item.PreferenceLevel,
                        SemesterId = request.ToSemesterId,
                        DepartmentHeadId = request.DepartmentHeadId
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

        public void CreateSubjectPreferenceForNewLecturer(Lecturer lecturer)
        {
            List<SubjectPreferenceLevel> subjectPreferenceLevel = new();
            var subjects = _unitOfWork.SubjectRepository.GetAll()
                .Where(item => item.SemesterId == lecturer.SemesterId && item.DepartmentHeadId == lecturer.DepartmentHeadId);
            foreach (var item in subjects)
            {
                subjectPreferenceLevel.Add(new SubjectPreferenceLevel()
                {
                    SubjectId = item.Id,
                    LecturerId = lecturer.Id,
                    PreferenceLevel = 0,
                    SemesterId = lecturer.SemesterId,
                    DepartmentHeadId = lecturer.DepartmentHeadId
                });
            }
            _unitOfWork.SubjectPreferenceLevelRepository.AddRange(subjectPreferenceLevel);
            _unitOfWork.Complete();
        }

        public ResponseResult CreateDefaultSubjectPreferenceLevel(GetAllRequest request)
        {
            try
            {
                var subjects = _unitOfWork.SubjectRepository
                    .GetAll()
                    .Where(item => item.SemesterId == request.SemesterId && item.DepartmentHeadId == request.DepartmentHeadId).ToList();
                if (subjects.Count == 0)
                {
                    return new ResponseResult("Must be create data of subjects first");
                }

                var lecturers = _unitOfWork.LecturerRepository
                    .GetAll()
                    .Where(item => item.SemesterId == request.SemesterId && item.DepartmentHeadId == request.DepartmentHeadId).ToList();

                if (lecturers.Count == 0)
                {
                    return new ResponseResult("Must be create data of lecturers first");
                }

                foreach (var item in lecturers)
                {
                    CreateSubjectPreferenceForNewLecturer(item);
                }

                return new ResponseResult("Create data successfully", true);
            }
            catch (Exception ex)
            {
                return new ResponseResult($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
