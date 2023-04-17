using Capstone_API.DTO.CommonRequest;
using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.TimeSlot.Request;
using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;


namespace Capstone_API.Controllers
{
    [Route("api/time-slot")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        private readonly ITimeSlotService _timeSlotService;
        private readonly ITimeSlotConflictService _timeSlotConflictService;
        private readonly IAreaSlotWeightService _areaSlotWeightService;

        public TimeSlotController(ITimeSlotService timeSlotService,
            ITimeSlotConflictService timeSlotConflictService,
            IAreaSlotWeightService areaSlotWeightService)
        {
            _timeSlotService = timeSlotService;
            _timeSlotConflictService = timeSlotConflictService;
            _areaSlotWeightService = areaSlotWeightService;
        }

        #region TimeSlot Api

        [HttpPost("get")]
        public GenericResult<List<TimeSlotResponse>> GetTimeSlots([FromBody] GetAllRequest request)
        {
            return _timeSlotService.GetAll(request);
        }

        [HttpGet("{id}")]
        public string GetTimeSlot()
        {
            return "value";
        }

        [HttpPost]
        public ResponseResult CreateTimeSlot([FromBody] CreateTimeSlotDTO request)
        {
            return _timeSlotService.CreateTimeSlot(request);
        }

        [HttpPut]
        public ResponseResult UpdateTimeslot([FromBody] UpdateTimeSlotDTO request)
        {
            return _timeSlotService.UpdateTimeslot(request);
        }

        [HttpDelete("{id}")]
        public ResponseResult DeleteTimeSlot(int id)
        {
            return _timeSlotService.DeleteTimeSlot(id);

        }

        [HttpPost("reuse")]
        public ResponseResult ReUseDataFromASemester([FromBody] ReUseRequest request)
        {
            return _timeSlotService.ReUseDataFromASemester(request);
        }

        #endregion

        #region TimeSlotSegmentApi

        [HttpPost("segment/get")]
        public GenericResult<List<GetSegmentResponseDTO>> GetAllTimeSlotSegment([FromBody] GetAllRequest request)
        {
            return _timeSlotService.GetTimeSlotSegment(request);
        }

        [HttpPost("segment")]
        public GenericResult<TimeSlotSegmentDTO> CreateTimeSlotSegment([FromBody] TimeSlotSegmentDTO request)
        {
            return _timeSlotService.CreateTimeSlotSegment(request);
        }

        [HttpPut("segment")]
        public GenericResult<TimeSlotSegmentDTO> UpdateTimeslotSegment([FromBody] TimeSlotSegmentDTO request)
        {
            return _timeSlotService.UpdateTimeslotSegment(request);
        }

        [HttpDelete("segment/{id}")]
        public ResponseResult DeleteTimeSlotSegment(int id)
        {
            return _timeSlotService.DeleteTimeSlotSegment(id);
        }

        #endregion

        #region TimeSlotConflict Api

        [HttpPost("conflict")]
        public GenericResult<List<GetTimeSlotConflictDTO>> GetAllTimeSlotConflict([FromBody] GetAllRequest request)
        {
            return _timeSlotConflictService.GetAll(request);
        }


        [HttpPut("conflict")]
        public ResponseResult UpdateTimeSlotConflict([FromBody] UpdateTimeSlotConflictDTO request)
        {
            return _timeSlotConflictService.UpdateTimeSlotConflict(request);
        }

        #endregion

        #region AreaSlotWeight Api

        [HttpPost("slot-weight")]
        public GenericResult<List<GetAreaSlotWeightDTO>> GetAllAreaTimeSlotWeight([FromBody] GetAllRequest request)
        {
            return _areaSlotWeightService.GetAll(request);
        }


        [HttpPut("slot-weight")]
        public ResponseResult UpdateAreaTimeSlotWeight([FromBody] UpdateAreaTimeSlotWeight request)
        {
            return _areaSlotWeightService.UpdateAreaTimeSlotWeight(request);
        }

        #endregion
    }
}
