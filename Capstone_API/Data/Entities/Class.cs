namespace Exercise.Data.Entities
{
    public class Class
    {

        public int Id { get; set; }
        public string? Name { get; set; }

        public Class(int id, string? name)
        {
            Id = id;
            Name = name;
        }
        public Class()
        {
        }

    }
}