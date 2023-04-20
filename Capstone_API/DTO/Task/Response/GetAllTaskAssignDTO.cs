namespace Capstone_API.DTO.Task.Response
{
    public class GetAllTaskAssignDTO
    {
        public GetAllTaskAssignDTO(int id, int? classId, int? subjectId, string? department, int? timeSlotId, string? slot1, string? slot2, int? semesterId, bool? status, int? lecturerId, int? room1Id, int? room2Id)
        {
            Id = id;
            ClassId = classId;
            SubjectId = subjectId;
            Department = department;
            TimeSlotId = timeSlotId;
            Slot1 = slot1;
            Slot2 = slot2;
            SemesterId = semesterId;
            Status = status;
            LecturerId = lecturerId;
            Room1Id = room1Id;
            Room2Id = room2Id;
        }

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

        public GetAllTaskAssignDTO()
        {
        }
    }
}
