using AutoMapper;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Capstone_API.UOW_Repositories.UnitOfWork;

namespace Capstone_API.Service.Implement
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TimeSlotService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GenericResult<IEnumerable<TimeSlotResponse>> GetAll()
        {
            try
            {
                var timeSlot = _unitOfWork.TimeSlotRepository.GetAll();
                var timeSlotsViewModel = _mapper.Map<IEnumerable<TimeSlotResponse>>(timeSlot);
                return new GenericResult<IEnumerable<TimeSlotResponse>>(timeSlotsViewModel, true);
            }
            catch (Exception ex)
            {
                return new GenericResult<IEnumerable<TimeSlotResponse>>($"{ex.Message}: {ex.InnerException?.Message}");
            }
        }
    }
}
