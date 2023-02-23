namespace Capstone_API.Data.Entities
{
    public class Semester
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Year { get; set; }

        public Semester(int id, string? name, int year)
        {
            Id = id;
            Name = name;
            Year = year;
        }

        public Semester()
        {
        }
    }
}
