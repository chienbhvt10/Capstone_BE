namespace Capstone_API.DTO.PreferenceLevel.Response
{
    public class GetSubjectPreferenceLevelDTO
    {

        public int LecturerId { get; set; }
        public int SemesterId { get; set; }
        public string? LecturerName { get; set; }
        public List<SubjectPreferenceInfo>? PreferenceInfos { get; set; }
    }

    public class SubjectPreferenceInfo
    {
        public int PreferenceId { get; set; }
        public int PreferenceLevel { get; set; }
        public int SubjectId { get; set; }
    }

    public class GetSubjectPreferenceLevelResponse
    {
        public List<GetSubjectPreferenceLevelDTO>? SubjectPreferenceLevels { get; set; }
        public int Total { get; set; }
    }
}
