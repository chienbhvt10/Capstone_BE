namespace Capstone_API.Data.Entities
{
    public class TimeSlotCompatibility
    {
        public int Id { get; set; }
        public int Slot_id { get; set; }
        public int Compatibility_slot_id { get; set; }
        public int Compatibility_level { get; set; }
        public int SemesterId { get; set; }

        public TimeSlotCompatibility(int id, int slot_id, int compatibility_slot_id, int compatibility_level, int semesterId)
        {
            Id = id;
            Slot_id = slot_id;
            Compatibility_slot_id = compatibility_slot_id;
            Compatibility_level = compatibility_level;
            SemesterId = semesterId;
        }

        public TimeSlotCompatibility()
        {
        }
    }
}
