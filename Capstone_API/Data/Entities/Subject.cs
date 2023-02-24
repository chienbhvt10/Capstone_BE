namespace Capstone_API.Data.Entities
{
    public class Subject
    {

        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int SemesterId { get; set; }
        public int OrderNumber { get; set; }
        public string? Department { get; set; }

        public Subject(int id, string? code, string? name, int semesterId, int orderNumber, string? department)
        {
            Id = id;
            Code = code;
            Name = name;
            SemesterId = semesterId;
            OrderNumber = orderNumber;
            Department = department;
        }

        public Subject()
        {
        }
    }
}
