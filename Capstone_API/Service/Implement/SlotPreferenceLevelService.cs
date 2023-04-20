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
    public class SlotPreferenceLevelService : ISlotPreferenceLevelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SlotPreferenceLevelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<List<GetSlotPreferenceLevelDTO>> GetAll(GetAllRequest request)
        {
            try
            {
                var query = SlotPreferenceLevelByLecturerIsKey(request);
                var slotViewModel = _mapper.Map<IEnumerable<GetSlotPreferenceLevelDTO>>(query).ToList();

                return new GenericResult<List<GetSlotPreferenceLevelDTO>>(slotViewModel, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<List<GetSlotPreferenceLevelDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public IEnumerable<GetSlotPreferenceLevelDTO> SlotPreferenceLevelByLecturerIsKey(GetAllRequest request)
        {
            var data = _unitOfWork.SlotPreferenceLevelRepository.MappingSlotPreferenceData()
                .Where(item => item.SemesterId == request.SemesterId && item.DepartmentHeadId == request.DepartmentHeadId)
                .OrderBy(item => item.LecturerId).GroupBy(item => item.LecturerId);
            var result = data.Select(group =>
                new GetSlotPreferenceLevelDTO
                {
                    LecturerId = group.Key ?? 0,
                    SemesterId = group.First().SemesterId ?? 0,
                    LecturerName = group.First().Lecturer?.ShortName ?? "",
                    PreferenceInfos = group.OrderBy(item => item.SlotId).Select(data =>
                        new SlotPreferenceInfo
                        {
                            PreferenceId = data.Id,
                            PreferenceLevel = data.PreferenceLevel ?? 0,
                            TimeSlotId = data.SlotId ?? 0
                        }).ToList(),
                }).ToList();
            return result;
        }

        public ResponseResult UpdateSlotPreferenceLevel(UpdateSlotPreferenceLevelDTO request)
        {
            try
            {
                var slotPreferenceLevel = _unitOfWork.SlotPreferenceLevelRepository.Find(item => item.Id == request.PreferenceId);
                slotPreferenceLevel.PreferenceLevel = request.PreferenceLevel;
                _unitOfWork.SlotPreferenceLevelRepository.Update(slotPreferenceLevel);
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

                var currentSemesterTimeSlot = _unitOfWork.TimeSlotRepository
                    .GetAll()
                    .Where(item =>
                        item.SemesterId == request.ToSemesterId
                        && item.DepartmentHeadId == request.DepartmentHeadId)
                    .ToList();
                if (currentSemesterTimeSlot.Count == 0)
                {
                    return new ResponseResult("Reuse fail, this semester have nodata of timeslots, must be reuse of timeslots first", false);
                }

                var fromTimeSlotPreferenceLevelData = _unitOfWork.SlotPreferenceLevelRepository
                    .GetAll()
                    .Where(item =>
                        item.SemesterId == request.FromSemesterId
                        && item.DepartmentHeadId == request.DepartmentHeadId)
                    .ToList();
                List<SlotPreferenceLevel> newSlotPreferenceLevel = new();

                foreach (var item in fromTimeSlotPreferenceLevelData)
                {
                    var lecturerNameInOldSemester = _unitOfWork.LecturerRepository.GetById(item.LecturerId ?? 0)?.ShortName;
                    var lecturerInCurrentSemester = _unitOfWork.LecturerRepository
                        .GetByCondition(item =>
                            item.SemesterId == request.ToSemesterId
                            && item.DepartmentHeadId == request.DepartmentHeadId
                            && item.ShortName == lecturerNameInOldSemester).FirstOrDefault();

                    var timeslotNameInOldSemester = _unitOfWork.TimeSlotRepository.GetById(item.SlotId ?? 0)?.Name;
                    var timeslotInCurrentSemester = _unitOfWork.TimeSlotRepository
                        .GetByCondition(item =>
                            item.SemesterId == request.ToSemesterId
                            && item.DepartmentHeadId == request.DepartmentHeadId
                            && item.Name == timeslotNameInOldSemester).FirstOrDefault();

                    newSlotPreferenceLevel.Add(new SlotPreferenceLevel()
                    {
                        LecturerId = lecturerInCurrentSemester?.Id,
                        SlotId = timeslotInCurrentSemester?.Id,
                        PreferenceLevel = item.PreferenceLevel,
                        SemesterId = request.ToSemesterId,
                        DepartmentHeadId = request.DepartmentHeadId
                    });
                }
                _unitOfWork.SlotPreferenceLevelRepository.AddRange(newSlotPreferenceLevel);
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
