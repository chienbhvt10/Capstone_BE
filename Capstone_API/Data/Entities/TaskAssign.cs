namespace Capstone_API.Data.Entities
{
    public class TaskAssign
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public string? Department { get; set; }
        public int TimeSlotId { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
        public string? Room1 { get; set; }
        public string? Room2 { get; set; }
        public int BuildingId { get; set; }
        public int SemesterId { get; set; }
        public bool Status { get; set; }

        public TaskAssign(int id, int classId, int subjectId, string? department, int timeSlotId, string? slot1, string? slot2, string? room, string? room2, int buildingID, int semesterID, bool status)
        {
            Id = id;
            ClassId = classId;
            SubjectId = subjectId;
            Department = department;
            TimeSlotId = timeSlotId;
            Slot1 = slot1;
            Slot2 = slot2;
            Room1 = room;
            Room2 = room2;
            BuildingId = buildingID;
            SemesterId = semesterID;
            Status = status;
        }

        public TaskAssign() { }
    }
}
