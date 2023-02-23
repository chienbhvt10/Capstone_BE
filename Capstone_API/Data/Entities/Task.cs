namespace Capstone_API.Data.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public string? Department { get; set; }
        public int TimeSlotId { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
        public string? Room { get; set; }
        public string? Room2 { get; set; }
        public int BuildingID { get; set; }
        public int SemesterID { get; set; }
        public string? Status { get; set; }

        public Task(int id, int classId, int subjectId, string? department, int timeSlotId, string? slot1, string? slot2, string? room, string? room2, int buildingID, int semesterID, string? status)
        {
            Id = id;
            ClassId = classId;
            SubjectId = subjectId;
            Department = department;
            TimeSlotId = timeSlotId;
            Slot1 = slot1;
            Slot2 = slot2;
            Room = room;
            Room2 = room2;
            BuildingID = buildingID;
            SemesterID = semesterID;
            Status = status;
        }

        public Task() { }
    }
}
