namespace Capstone_API.Data.Entities
{
    public class TimeSLotConflict
    {
        public int Id { get; set; }
        public int Slot_id { get; set; }
        public int Conflict_slot_id { get; set; }
        public int Conflict { get; set; }
        public int SemesterId { get; set; }

        public TimeSLotConflict(int id, int slot_id, int conflict_slot_id, int conflict, int semesterId)
        {
            Id = id;
            Slot_id = slot_id;
            Conflict_slot_id = conflict_slot_id;
            Conflict = conflict;
            SemesterId = semesterId;
        }

        public TimeSLotConflict()
        {
        }
    }
}
