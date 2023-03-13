namespace Capstone_API.DTO.TimeSlot.Response
{
    public class TimeSlotResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SemesterId { get; set; }
        public int? OrderNumber { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
    }
}
