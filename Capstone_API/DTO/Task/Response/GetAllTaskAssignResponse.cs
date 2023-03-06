namespace Capstone_API.DTO.Task.Response
{
    public class GetAllTaskAssignResponse
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public string? Department { get; set; }
        public int? TimeSlotId { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
        public int? SemesterId { get; set; }
        public bool? Status { get; set; }
        public int? LecturerId { get; set; }
        public int? Room1Id { get; set; }
        public int? Room2Id { get; set; }
    }
}
