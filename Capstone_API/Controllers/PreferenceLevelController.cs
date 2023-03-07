using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/preference-level")]
    [ApiController]
    public class PreferenceLevelController : ControllerBase
    {
        #region Slot PreferenceLevel Api

        [HttpGet("slot")]
        public IEnumerable<string> GetSlotPreferenceLevels()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPut("slot")]
        public void UpdateSlotPreferenceLevels()
        {
        }

        #endregion

        #region Subject PreferenceLevel Api

        [HttpGet("subject")]
        public IEnumerable<string> GetSubjectPreferenceLevels()
        {
            return new string[] { "value1", "value2" };
        }



        [HttpPut("subject")]
        public void UpdateSubjectPreferenceLevels()
        {
        }

        #endregion
    }
}
