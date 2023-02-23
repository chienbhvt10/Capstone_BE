namespace Capstone_API.Data.Entities
{
    public class Lecturer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Short_name { get; set; }
        public int Preference_level { get; set; }
        public int SemesterId { get; set; }
        public int OrderNumber { get; set; }

        public Lecturer(int id, string? name, string? short_name, int preference_level, int semesterId, int orderNumber)
        {
            Id = id;
            Name = name;
            Short_name = short_name;
            Preference_level = preference_level;
            SemesterId = semesterId;
            OrderNumber = orderNumber;
        }

        public Lecturer()
        {
        }
    }
}
