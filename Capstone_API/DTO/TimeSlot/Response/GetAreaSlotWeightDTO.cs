namespace Capstone_API.DTO.TimeSlot.Response
{
    public class GetAreaSlotWeightDTO
    {
        public int TimeSlotId { get; set; }
        public int SemesterId { get; set; }
        public string? TimeSlotName { get; set; }
        public List<AreaSlotWeightInfo>? AreaSlotWeightInfos { get; set; }
    }

    public class AreaSlotWeightInfo
    {
        public int SlotWeightId { get; set; }
        public int SlotWeight { get; set; }
        public int TimeSlotId { get; set; }
    }
}
