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
