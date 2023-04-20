namespace Capstone_API.DTO.TimeSlot.Request
{
    public class TimeSlotSegmentDTO
    {
        public int SegmentId { get; set; }
        public int? SlotId { get; set; }
        public int? DayOfWeek { get; set; }
        public int? Segment { get; set; }
        public int? SemesterId { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
