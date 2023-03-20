using AutoMapper;
using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class TimeSlotCompatibilityService : ITimeSlotCompatibilityService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TimeSlotCompatibilityService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public GenericResult<List<GetTimeSlotCompatibilityDTO>> GetAll()
        {
            try
            {
                var query = TimeSlotCompatibilityByTimeSlotIsKey();
                var timeSlotCompatibilityViewModel = _mapper.Map<IEnumerable<GetTimeSlotCompatibilityDTO>>(query).ToList();

                return new GenericResult<List<GetTimeSlotCompatibilityDTO>>(timeSlotCompatibilityViewModel, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<List<GetTimeSlotCompatibilityDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public IEnumerable<GetTimeSlotCompatibilityDTO> TimeSlotCompatibilityByTimeSlotIsKey()
        {
            var data = _unitOfWork.TimeSlotCompatibilityRepository.TimeSlotData()
                .OrderBy(item => item.SlotId).GroupBy(item => item.SlotId);

            var result = data.Select(group =>
                new GetTimeSlotCompatibilityDTO
                {
                    TimeslotId = group.First().SlotId ?? 0,
                    SemesterId = group.First().SemesterId ?? 0,
                    TimeSlotName = group.First().Slot?.Name ?? "",
                    SlotCompatibilityInfos = group.OrderBy(item => item.CompatibilitySlotId).Select(data =>
                        new SlotCompatibilityInfo
                        {
                            CompatibilityId = data.Id,
                            CompatibilityLevel = data.CompatibilityLevel ?? 0,
                            TimeSlotId = data.SlotId ?? 0
                        }).ToList(),
                }).ToList();
            return result;
        }

        public ResponseResult UpdateTimeSlotCompatibility(UpdateTimeSlotCompatibilityDTO request)
        {
            try
            {
                var slotCompatibility = _unitOfWork.TimeSlotCompatibilityRepository.Find(item => item.Id == request.CompatibilityId);
                slotCompatibility.CompatibilityLevel = request.CompatibilityLevel;
                _unitOfWork.TimeSlotCompatibilityRepository.Update(slotCompatibility);
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
