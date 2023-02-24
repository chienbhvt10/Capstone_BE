namespace Capstone_API.Data.Entities
{
    public class SlotPreferenceLevel
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int SlotId { get; set; }
        public int PreferenceLevel { get; set; }
        public int SemesterId { get; set; }

        public SlotPreferenceLevel(int id, int lecturer_id, int subject_id, int preference_level, int semesterId)
        {
            Id = id;
            LecturerId = lecturer_id;
            SlotId = subject_id;
            PreferenceLevel = preference_level;
            SemesterId = semesterId;
        }

        public SlotPreferenceLevel()
        {
        }
    }
}
