namespace Capstone_API.Data.Entities
{
    public class Subject
    {

        public int Id { get; set; }
        public int Code { get; set; }
        public int Name { get; set; }
        public int SemesterId { get; set; }
        public int OrderNumber { get; set; }
        public int Department { get; set; }

        public Subject(int id, int code, int name, int semesterId, int orderNumber, int department)
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
