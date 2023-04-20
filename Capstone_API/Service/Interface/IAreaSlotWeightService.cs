using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IAreaSlotWeightService
    {
        GenericResult<List<GetAreaSlotWeightDTO>> GetAll(GetAllRequest request);
        ResponseResult UpdateAreaTimeSlotWeight(UpdateAreaTimeSlotWeight request);
    }
}
