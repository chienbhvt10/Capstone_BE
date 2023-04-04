namespace Capstone_API.DTO.TimeSlot.Request
{
    public class CreateTimeSlotDTO
    {
        public int DaySession { get; set; }
        public string? Name { get; set; }
        public List<CreateSegmentData>? Segments { get; set; }
    }

    public class CreateSegmentData
    {
        public int Segment { get; set; }
        public int Day { get; set; }
    }
}
