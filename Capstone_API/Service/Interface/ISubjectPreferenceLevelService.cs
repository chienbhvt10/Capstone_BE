using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.PreferenceLevel.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface ISubjectPreferenceLevelService
    {
        GenericResult<List<GetSubjectPreferenceLevelDTO>> GetAll();
        ResponseResult UpdateSubjectPreferenceLevel(UpdateSubjectPreferenceLevelDTO request);
    }
}
