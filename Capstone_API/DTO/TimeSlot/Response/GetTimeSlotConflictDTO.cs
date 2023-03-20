namespace Capstone_API.DTO.TimeSlot.Response
{
    public class GetTimeSlotConflictDTO
    {
        public int TimeslotId { get; set; }
        public int SemesterId { get; set; }
        public string? TimeSlotName { get; set; }
        public List<SlotConflictInfo>? SlotConflictInfos { get; set; }
    }

    public class SlotConflictInfo
    {
        public int ConflictId { get; set; }
        public bool Conflict { get; set; }
        public int TimeSlotId { get; set; }
    }
}
