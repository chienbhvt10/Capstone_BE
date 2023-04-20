namespace Capstone_API.DTO.TimeSlot.Request
{
    public class UpdateTimeSlotDTO
    {
        public int Id { get; set; }
        public int? AmorPm { get; set; }
        public string? Name { get; set; }
        public int SemesterId { get; set; }
    }
}
