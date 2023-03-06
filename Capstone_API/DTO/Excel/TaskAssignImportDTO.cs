namespace Capstone_API.DTO.Excel
{
    public class TaskAssignImportDTO
    {
        public string? ClassName { get; set; } = string.Empty;
        public string? SubjectName { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public string? TimeSlot { get; set; } = string.Empty;
        public string? Slot1 { get; set; } = string.Empty;
        public string? Slot2 { get; set; } = string.Empty;
        public string? Room1 { get; set; } = string.Empty;
        public string? Room2 { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;

        public TaskAssignImportDTO(string? className, string? subjectName, string? department, string? timeSlot, string? slot1, string? slot2, string? room1, string? room2, string? status)
        {
            ClassName = className;
            SubjectName = subjectName;
            Department = department;
            TimeSlot = timeSlot;
            Slot1 = slot1;
            Slot2 = slot2;
            Room1 = room1;
            Room2 = room2;
            Status = status;
        }

        public TaskAssignImportDTO()
        {
        }
    }
}
