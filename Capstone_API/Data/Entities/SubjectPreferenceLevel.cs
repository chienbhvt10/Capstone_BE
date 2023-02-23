namespace Capstone_API.Data.Entities
{
    public class SubjectPreferenceLevel
    {
        public int Id { get; set; }
        public int Lecturer_id { get; set; }
        public int Subject_id { get; set; }
        public int Preference_level { get; set; }
        public int SemesterId { get; set; }

        public SubjectPreferenceLevel(int id, int lecturer_id, int subject_id, int preference_level, int semesterId)
        {
            Id = id;
            Lecturer_id = lecturer_id;
            Subject_id = subject_id;
            Preference_level = preference_level;
            SemesterId = semesterId;
        }

        public SubjectPreferenceLevel()
        {
        }
    }
}
