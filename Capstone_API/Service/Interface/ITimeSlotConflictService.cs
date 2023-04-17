using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ITimeSlotConflictService
    {
        GenericResult<List<GetTimeSlotConflictDTO>> GetAll(GetAllRequest request);
        ResponseResult UpdateTimeSlotConflict(UpdateTimeSlotConflictDTO request);
    }
}
