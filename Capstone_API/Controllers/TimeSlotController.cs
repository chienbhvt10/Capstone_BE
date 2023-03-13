using Capstone_API.DTO.TimeSlot.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/time-slot")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {


        private readonly ITimeSlotService _timeSlotService;

        public TimeSlotController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
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
        public IEnumerable<string> GetTimeSlotCompatibilities()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPut("timeslot-compatibility/{id}")]
        public void UpdateTimeSlotCompatibility()
        {
        }

        #endregion

        #region TimeSlotConflict Api

        [HttpGet("timeslot-conflict")]
        public IEnumerable<string> GetTimeSlotConflict()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPut("timeslot-conflict/{id}")]
        public void UpdateTimeSlotConflict()
        {
        }

        #endregion
    }
}
