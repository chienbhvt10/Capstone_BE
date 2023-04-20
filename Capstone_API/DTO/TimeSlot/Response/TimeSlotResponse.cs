namespace Capstone_API.DTO.TimeSlot.Response
{
    public class TimeSlotResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? AmorPm { get; set; }
        public int? SemesterId { get; set; }
    }
}
