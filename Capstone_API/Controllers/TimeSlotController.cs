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
        private readonly ITimeSlotCompatibilityService _timeSlotCompatibilityService;
        private readonly ITimeSlotConflictService _timeSlotConflictService;
        private readonly IAreaSlotWeightService _areaSlotWeightService;

        public TimeSlotController(ITimeSlotService timeSlotService,
            ITimeSlotCompatibilityService timeSlotCompatibilityService,
            ITimeSlotConflictService timeSlotConflictService,
            IAreaSlotWeightService areaSlotWeightService)
        {
            _timeSlotService = timeSlotService;
            _timeSlotCompatibilityService = timeSlotCompatibilityService;
            _timeSlotConflictService = timeSlotConflictService;
            _areaSlotWeightService = areaSlotWeightService;
        }

        #region TimeSlot Api

        [HttpGet]
        public GenericResult<IEnumerable<TimeSlotResponse>> GetTimeSlots()
        {
            return _timeSlotService.GetAll();
        }

        [HttpGet("{id}")]
        public string GetTimeSlot()
        {
            return "value";
        }

        [HttpPost]
        public void CreateTimeSlot()
        {
        }

        [HttpPut("{id}")]
        public void UpdateTimeSlot()
        {
        }

        [HttpDelete("{id}")]
        public void DeleteTimeSlot()
        {
        }

        #endregion

        #region TimeSlotCompatibility Api

        [HttpGet("timeslot-compatibility")]
        public GenericResult<List<GetTimeSlotCompatibilityDTO>> GetAllTimeSlotCompatibility()
        {
            return _timeSlotCompatibilityService.GetAll();
        }

        [HttpPut("timeslot-compatibility")]
        public ResponseResult UpdateTimeSlotCompatibility([FromBody] UpdateTimeSlotCompatibilityDTO request)
        {
            return _timeSlotCompatibilityService.UpdateTimeSlotCompatibility(request);
        }

        #endregion

        #region TimeSlotConflict Api

        [HttpGet("timeslot-conflict")]
        public GenericResult<List<GetTimeSlotConflictDTO>> GetAllTimeSlotConflict()
        {
            return _timeSlotConflictService.GetAll();
        }


        [HttpPut("timeslot-conflict")]
        public ResponseResult UpdateTimeSlotConflict([FromBody] UpdateTimeSlotCompatibilityDTO request)
        {
            return _timeSlotCompatibilityService.UpdateTimeSlotCompatibility(request);
        }

        #endregion

        #region AreaSlotWeight Api

        [HttpGet("area-time-slot-weight")]
        public GenericResult<List<GetAreaSlotWeightDTO>> GetAllAreaTimeSlotWeight()
        {
            return _areaSlotWeightService.GetAll();
        }


        [HttpPut("area-time-slot-weight")]
        public ResponseResult UpdateAreaTimeSlotWeight([FromBody] UpdateAreaTimeSlotWeight request)
        {
            return _areaSlotWeightService.UpdateAreaTimeSlotWeight(request);
        }

        #endregion
    }
}
