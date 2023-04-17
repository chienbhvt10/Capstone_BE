using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.PreferenceLevel.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ISubjectPreferenceLevelService
    {
        GenericResult<List<GetSubjectPreferenceLevelDTO>> GetAll(GetAllRequest request);
        ResponseResult UpdateSubjectPreferenceLevel(UpdateSubjectPreferenceLevelDTO request);
        ResponseResult ReUseDataFromASemester(ReUseRequest request);
    }
}
