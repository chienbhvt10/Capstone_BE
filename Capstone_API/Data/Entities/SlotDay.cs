namespace Capstone_API.Data.Entities
{
    public class SlotDay
    {
        public int Id { get; set; }
        public int NumberOfSlots { get; set; }
        public int SemesterId { get; set; }

        public SlotDay(int id, int numberOfSlots, int semesterId)
        {
            Id = id;
            NumberOfSlots = numberOfSlots;
            SemesterId = semesterId;
        }

        public SlotDay()
        {
        }
    }
}
