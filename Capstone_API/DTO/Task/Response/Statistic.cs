namespace Capstone_API.DTO.Task.Response
{
    public class Statistic
    {
        public List<int>? CountGroupByTimeSlot { get; set; }
        public List<int>? CountAllGroupByTimeSlot { get; set; }
        public List<int>? CountAllGroupBySubject { get; set; }
        public int AssignedCount { get; set; }
        public int NotAssignedCount { get; set; }
        public int TotalTask { get; set; }
    }
}
