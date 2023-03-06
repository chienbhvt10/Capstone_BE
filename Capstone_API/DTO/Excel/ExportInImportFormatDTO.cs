namespace Capstone_API.DTO.Excel
{
    public class ExportInImportFormatDTO
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
        public string? Lecturer { get; set; } = string.Empty;


        public ExportInImportFormatDTO(string? className, string? subjectName, string? department, string? timeSlot, string? slot1, string? slot2, string? room1, string? room2, string? status, string? lecturer)
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
            Lecturer = lecturer;
        }

        public ExportInImportFormatDTO()
        {
        }
    }
}
