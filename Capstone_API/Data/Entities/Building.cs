namespace Capstone_API.Data.Entities
{
    public class Building
    {

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public Building() { }

        public Building(int id, string? name, string? short_name)
        {
            Id = id;
            Name = name;
            ShortName = short_name;
        }
    }
}
