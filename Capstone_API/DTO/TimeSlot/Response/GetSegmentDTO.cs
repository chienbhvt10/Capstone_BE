namespace Capstone_API.DTO.TimeSlot.Response
{
    public class GetSegmentDTO
    {
        public int TimeSlotId { get; set; }
        public int? AmorPm { get; set; }
        public string? TimeSlotName { get; set; }
        public int? SemesterId { get; set; }
        public int? DayId { get; set; }
        public string? Day { get; set; }
        public int? SegmentId { get; set; }
        public int? Segment { get; set; }
    }

    public class GetSegmentResponseDTO
    {
        public int TimeSlotId { get; set; }
        public string? TimeSlotName { get; set; }
        public int SemesterId { get; set; }
        public int? AmorPm { get; set; }
        public List<SlotSegment>? SlotSegments { get; set; }

    }
    public class SlotSegment
    {
        public int? SegmentId { get; set; }
        public int? SlotId { get; set; }
        public int DayId { get; set; }
        public string? Day { get; set; }
        public int? Segment { get; set; }
    }
}
