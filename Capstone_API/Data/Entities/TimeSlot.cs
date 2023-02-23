namespace Capstone_API.Data.Entities
{
    public class TimeSlot
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int SemesterId { get; set; }
        public int OrderNumber { get; set; }

        public TimeSlot(int id, string? name, string? description, int semesterId, int orderNumber)
        {
            Id = id;
            Name = name;
            Description = description;
            SemesterId = semesterId;
            OrderNumber = orderNumber;
        }

        public TimeSlot() { }
    }
}
