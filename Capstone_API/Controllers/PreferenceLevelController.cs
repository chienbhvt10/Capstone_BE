using Capstone_API.DTO.CommonRequest;
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

        [HttpPost("slot")]
        public GenericResult<List<GetSlotPreferenceLevelDTO>> GetAllSlotPreferenceLevel([FromBody] GetAllRequest request)
        {
            return _slotPreferenceLevelService.GetAll(request);
        }

        [HttpPut("slot")]
        public ResponseResult UpdateSlotPreferenceLevels([FromBody] UpdateSlotPreferenceLevelDTO request)
        {
            return _slotPreferenceLevelService.UpdateSlotPreferenceLevel(request);
        }

        [HttpPost("slot/reuse")]
        public ResponseResult ReUseSlotPreferenceDataFromASemester([FromBody] ReUseRequest request)
        {
            return _slotPreferenceLevelService.ReUseDataFromASemester(request);
        }

        [HttpPost("slot/create-default")]
        public ResponseResult CreateDefaultSlotPreferenceLevel(GetAllRequest request)
        {
            return _slotPreferenceLevelService.CreateDefaultSlotPreferenceLevel(request);
        }

        #endregion

        #region Subject PreferenceLevel Api

        [HttpPost("subject")]
        public GenericResult<List<GetSubjectPreferenceLevelDTO>> GetAllSubjectPreferenceLevel([FromBody] GetAllRequest request)
        {
            return _subjectPreferenceLevelService.GetAll(request);
        }


        [HttpPut("subject")]
        public ResponseResult UpdateSubjectPreferenceLevels([FromBody] UpdateSubjectPreferenceLevelDTO request)
        {
            return _subjectPreferenceLevelService.UpdateSubjectPreferenceLevel(request);
        }

        [HttpPost("subject/reuse")]
        public ResponseResult ReUseSubjectPreferenceDataFromASemester([FromBody] ReUseRequest request)
        {
            return _subjectPreferenceLevelService.ReUseDataFromASemester(request);
        }

        #endregion
    }
}
