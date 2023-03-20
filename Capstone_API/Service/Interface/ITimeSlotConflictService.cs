using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITimeSlotConflictService
    {
        GenericResult<List<GetTimeSlotConflictDTO>> GetAll();
        ResponseResult UpdateTimeSlotConflict(UpdateTimeSlotConflictDTO request);
    }
}
