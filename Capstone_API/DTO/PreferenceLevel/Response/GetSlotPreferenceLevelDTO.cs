namespace Capstone_API.DTO.PreferenceLevel.Response
{
    public class GetSlotPreferenceLevelDTO
    {

        public int LecturerId { get; set; }
        public int SemesterId { get; set; }
        public string? LecturerName { get; set; }
        public List<SlotPreferenceInfo>? PreferenceInfos { get; set; }
    }

    public class SlotPreferenceInfo
    {
        public int PreferenceId { get; set; }
        public int PreferenceLevel { get; set; }
        public int TimeSlotId { get; set; }
    }
    public class GetSlotPreferenceLevelResponse
    {
        public List<GetSlotPreferenceLevelDTO>? SlotPreferenceLevels { get; set; }
        public int Total { get; set; }
    }
}
