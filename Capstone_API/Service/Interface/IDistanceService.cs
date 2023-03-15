using Capstone_API.DTO.Distance.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IDistanceService
    {
        GenericResult<List<RoomResponse>> GetAll();
    }
}
