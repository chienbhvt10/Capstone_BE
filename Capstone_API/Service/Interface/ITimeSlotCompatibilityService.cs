using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITimeSlotCompatibilityService
    {
        GenericResult<List<GetTimeSlotCompatibilityDTO>> GetAll();
        ResponseResult UpdateTimeSlotCompatibility(UpdateTimeSlotCompatibilityDTO request);
    }
}
