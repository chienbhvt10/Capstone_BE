namespace Capstone_API.Data.Entities
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int SemesterId { get; set; }
        public int OrderNumber { get; set; }

        public Lecturer(int id, string? name, string? short_name, int semesterId, int orderNumber)
        {
            Id = id;
            Name = name;
            ShortName = short_name;
            SemesterId = semesterId;
            OrderNumber = orderNumber;
        }

        public Lecturer()
        {
        }
    }
}
