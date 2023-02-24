namespace Capstone_API.Data.Entities
{
    public class TimeSLotConflict
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public int ConflictSlotId { get; set; }
        public bool Conflict { get; set; }
        public int SemesterId { get; set; }

        public TimeSLotConflict(int id, int slot_id, int conflict_slot_id, bool conflict, int semesterId)
        {
            Id = id;
            SlotId = slot_id;
            ConflictSlotId = conflict_slot_id;
            Conflict = conflict;
            SemesterId = semesterId;
        }

        public TimeSLotConflict()
        {
        }
    }
}
