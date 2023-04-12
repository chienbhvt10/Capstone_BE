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

        [HttpPost]
        public GenericResult<List<TimeSlotResponse>> GetTimeSlots([FromBody] int semesterId)
        {
            return _timeSlotService.GetAll(semesterId);
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
        #endregion

        #region TimeSlotSegmentApi

        [HttpPost("segment")]
        public GenericResult<List<GetSegmentResponseDTO>> GetAllTimeSlotSegment([FromBody] int semesterId)
        {
            return _timeSlotService.GetTimeSlotSegment(semesterId);
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

        [HttpGet("conflict")]
        public GenericResult<List<GetTimeSlotConflictDTO>> GetAllTimeSlotConflict()
        {
            return _timeSlotConflictService.GetAll();
        }


        [HttpPut("conflict")]
        public ResponseResult UpdateTimeSlotConflict([FromBody] UpdateTimeSlotConflictDTO request)
        {
            return _timeSlotConflictService.UpdateTimeSlotConflict(request);
        }

        #endregion

        #region AreaSlotWeight Api

        [HttpGet("slot-weight")]
        public GenericResult<List<GetAreaSlotWeightDTO>> GetAllAreaTimeSlotWeight()
        {
            return _areaSlotWeightService.GetAll();
        }


        [HttpPut("slot-weight")]
        public ResponseResult UpdateAreaTimeSlotWeight([FromBody] UpdateAreaTimeSlotWeight request)
        {
            return _areaSlotWeightService.UpdateAreaTimeSlotWeight(request);
        }

        #endregion
    }
}
