using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.Distance;
using Capstone_API.DTO.Distance.Request;
using Capstone_API.DTO.Distance.Response;
using Capstone_API.Results;

namespace Capstone_API.Service.Interface
{
    public interface IDistanceService
    {
        GenericResult<List<RoomResponse>> GetAllRoom(GetAllRequest request);
        ResponseResult UpdateDistance(UpdateDistanceDTO request);
        GenericResult<List<DistanceResponse>> GetAllDistance(GetAllRequest request);
        GenericResult<List<BuildingResponse>> GetAllBuilding(GetAllRequest request);
        ResponseResult CreateBuilding(CreateBuildingDTO request);
        ResponseResult UpdateBuilding(UpdateBuildingDTO request);
        ResponseResult DeleteBuilding(int id);

    }
}
