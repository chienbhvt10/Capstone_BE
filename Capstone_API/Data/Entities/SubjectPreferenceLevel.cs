namespace Capstone_API.Data.Entities
{
    public class SubjectPreferenceLevel
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
        public int PreferenceLevel { get; set; }
        public int SemesterId { get; set; }

        public SubjectPreferenceLevel(int id, int lecturer_id, int subject_id, int preference_level, int semesterId)
        {
            Id = id;
            LecturerId = lecturer_id;
            SubjectId = subject_id;
            PreferenceLevel = preference_level;
            SemesterId = semesterId;
        }

        public SubjectPreferenceLevel()
        {
        }
    }
}
