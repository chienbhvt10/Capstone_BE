using AutoMapper;
using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class TimeSlotConflictService : ITimeSlotConflictService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TimeSlotConflictService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<List<GetTimeSlotConflictDTO>> GetAll()
        {
            try
            {
                var query = TimeSlotConflictByTimeSlotIsKey();
                var timeSlotConflictViewModel = _mapper.Map<IEnumerable<GetTimeSlotConflictDTO>>(query).ToList();

                return new GenericResult<List<GetTimeSlotConflictDTO>>(timeSlotConflictViewModel, true);

            }
            catch (Exception ex)
            {
                return new GenericResult<List<GetTimeSlotConflictDTO>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }

        public IEnumerable<GetTimeSlotConflictDTO> TimeSlotConflictByTimeSlotIsKey()
        {
            var data = _unitOfWork.TimeSlotConflictRepository.TimeSlotData()
                .OrderBy(item => item.SlotId).GroupBy(item => item.SlotId);

            var result = data.Select(group =>
                new GetTimeSlotConflictDTO
                {
                    TimeSlotId = group.First().SlotId ?? 0,
                    SemesterId = group.First().SemesterId ?? 0,
                    TimeSlotName = group.First().Slot?.Name ?? "",
                    SlotConflictInfos = group.OrderBy(item => item.ConflictSlotId).Select(data =>
                        new SlotConflictInfo
                        {
                            ConflictId = data.Id,
                            Conflict = data.Conflict ?? false,
                            TimeSlotId = data.SlotId ?? 0
                        }).ToList(),
                }).ToList();
            return result;
        }

        public ResponseResult UpdateTimeSlotConflict(UpdateTimeSlotConflictDTO request)
        {
            try
            {
                var slotConflict = _unitOfWork.TimeSlotConflictRepository.Find(item => item.Id == request.ConflictId);
                slotConflict.Conflict = request.Conflict;
                _unitOfWork.TimeSlotConflictRepository.Update(slotConflict);
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
