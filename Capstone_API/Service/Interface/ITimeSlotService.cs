using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITimeSlotService
    {
        GenericResult<IEnumerable<TimeSlotResponse>> GetAll();
    }
}
