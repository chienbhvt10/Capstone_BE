using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/timeslot")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        #region TimeSlot Api

        [HttpGet]
        public IEnumerable<string> GetTimeSlots()
        {
            return new string[] { "value1", "value2" };
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
