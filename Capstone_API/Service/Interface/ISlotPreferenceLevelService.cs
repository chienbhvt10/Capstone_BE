using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.PreferenceLevel.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ISlotPreferenceLevelService
    {
        GenericResult<GetSlotPreferenceLevelResponse> GetAll(GetSlotPreferenceRequest request);
        ResponseResult UpdateSlotPreferenceLevel(UpdateSlotPreferenceLevelDTO request);
        ResponseResult ReUseDataFromASemester(ReUseRequest request);
        ResponseResult CreateDefaultSlotPreferenceLevel(GetAllRequest request);
    }
}
