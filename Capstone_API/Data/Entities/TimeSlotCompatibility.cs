namespace Capstone_API.Data.Entities
{
    public class TimeSlotCompatibility
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public int CompatibilitySlotId { get; set; }
        public int CompatibilityLevel { get; set; }
        public int SemesterId { get; set; }

        public TimeSlotCompatibility(int id, int slot_id, int compatibility_slot_id, int compatibility_level, int semesterId)
        {
            Id = id;
            SlotId = slot_id;
            CompatibilitySlotId = compatibility_slot_id;
            CompatibilityLevel = compatibility_level;
            SemesterId = semesterId;
        }

        public TimeSlotCompatibility()
        {
        }
    }
}
