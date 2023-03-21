using Capstone_API.DTO.Distance;
using Capstone_API.DTO.Distance.Request;
using Capstone_API.DTO.Distance.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IDistanceService
    {
        GenericResult<List<RoomResponse>> GetAllRoom();
        ResponseResult UpdateDistance(UpdateDistanceDTO request);
        GenericResult<List<DistanceResponse>> GetAllDistance();
        GenericResult<List<BuildingResponse>> GetAllBuilding();
        ResponseResult CreateBuilding(CreateBuildingDTO request);
    }
}
