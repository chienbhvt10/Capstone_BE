namespace Capstone_API.DTO.TimeSlot.Response
{
    public class GetTimeSlotCompatibilityDTO
    {
        public int TimeslotId { get; set; }
        public int SemesterId { get; set; }
        public string? TimeSlotName { get; set; }
        public List<SlotCompatibilityInfo>? SlotCompatibilityInfos { get; set; }
    }

    public class SlotCompatibilityInfo
    {
        public int CompatibilityId { get; set; }
        public int CompatibilityLevel { get; set; }
        public int TimeSlotId { get; set; }
    }
}
