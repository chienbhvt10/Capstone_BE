using Capstone_API.DTO.PreferenceLevel.Request;
using Capstone_API.DTO.PreferenceLevel.Response;
using Capstone_API.Results;
using Capstone_API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Capstone_API.Controllers
{
    [Route("api/preference-level")]
    [ApiController]
    public class PreferenceLevelController : ControllerBase
    {
        private readonly ISubjectPreferenceLevelService _subjectPreferenceLevelService;
        private readonly ISlotPreferenceLevelService _slotPreferenceLevelService;


        public PreferenceLevelController(
            ISubjectPreferenceLevelService subjectPreferenceLevelService,
            ISlotPreferenceLevelService slotPreferenceLevelRepository)
        {
            _subjectPreferenceLevelService = subjectPreferenceLevelService;
            _slotPreferenceLevelService = slotPreferenceLevelRepository;
        }
        #region Slot PreferenceLevel Api

        [HttpGet("slot")]
        public GenericResult<List<GetSlotPreferenceLevelDTO>> GetAllSlotPreferenceLevel()
        {
            return _slotPreferenceLevelService.GetAll();
        }

        [HttpPut("slot")]
        public ResponseResult UpdateSlotPreferenceLevels([FromBody] UpdateSlotPreferenceLevelDTO request)
        {
            return _slotPreferenceLevelService.UpdateSlotPreferenceLevel(request);
        }

        #endregion

        #region Subject PreferenceLevel Api

        [HttpGet("subject")]
        public GenericResult<List<GetSubjectPreferenceLevelDTO>> GetAllSubjectPreferenceLevel()
        {
            return _subjectPreferenceLevelService.GetAll();
        }


        [HttpPut("subject")]
        public ResponseResult UpdateSubjectPreferenceLevels([FromBody] UpdateSubjectPreferenceLevelDTO request)
        {
            return _subjectPreferenceLevelService.UpdateSubjectPreferenceLevel(request);
        }

        #endregion
    }
}
