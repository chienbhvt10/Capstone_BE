using Capstone_API.DTO.Distance;
using Capstone_API.DTO.Distance.Request;
using Capstone_API.DTO.Distance.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/distance")]
    [ApiController]
    public class DistanceController : ControllerBase
    {
        private readonly IDistanceService _distanceService;
        public DistanceController(IDistanceService distanceService)
        {
            _distanceService = distanceService;
        }

        #region Distance
        [HttpGet]
        public GenericResult<List<DistanceResponse>> GetAllDistance()
        {
            return _distanceService.GetAllDistance();
        }

        [HttpPut]
        public ResponseResult UpdateDistance(UpdateDistanceDTO request)
        {
            return _distanceService.UpdateDistance(request);
        }
        #endregion

        #region Building

        [HttpGet("building")]
        public GenericResult<List<BuildingResponse>> GetAllBuilding()
        {
            return _distanceService.GetAllBuilding();
        }

        [HttpPost("building")]
        public ResponseResult CreateBuilding(CreateBuildingDTO request)
        {
            return _distanceService.CreateBuilding(request);
        }

        [HttpPut("building")]
        public ResponseResult UpdateBuilding([FromBody] UpdateBuildingDTO request)
        {
            return _distanceService.UpdateBuilding(request);
        }

        [HttpDelete("building/{id}")]
        public ResponseResult DeleteBuilding(int id)
        {
            return _distanceService.DeleteBuilding(id);

        }

        #endregion

        #region Room
        [HttpGet("room")]
        public GenericResult<List<RoomResponse>> GetRooms()
        {
            return _distanceService.GetAllRoom();
        }
        #endregion
    }
}
