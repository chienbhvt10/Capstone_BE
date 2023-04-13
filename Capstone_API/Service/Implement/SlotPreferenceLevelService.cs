using AutoMapper;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.PreferenceLevel.Response;
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

        public GenericResult<List<GetSlotPreferenceLevelDTO>> GetAll(GetPreferenceRequest request)
        {
            try
            {
                var query = SubjectPreferenceLevelByLecturerIsKey(request.SemesterId);
                var subjectsViewModel = _mapper.Map<IEnumerable<GetSlotPreferenceLevelDTO>>(query).ToList();

                return new GenericResult<List<GetSlotPreferenceLevelDTO>>(subjectsViewModel, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<List<GetSlotPreferenceLevelDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
        public IEnumerable<GetSlotPreferenceLevelDTO> SubjectPreferenceLevelByLecturerIsKey(int semesterId)
        {
            var data = _unitOfWork.SlotPreferenceLevelRepository.MappingSlotPreferenceData()
                .Where(item => item.SemesterId == semesterId)
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
    }
}
