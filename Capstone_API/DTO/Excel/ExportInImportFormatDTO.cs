namespace Capstone_API.DTO.Excel
{
    public class ExportInImportFormatDTO
    {
        public string? Class { get; set; } = string.Empty;
        public string? Subject { get; set; } = string.Empty;
        public string? Dept { get; set; } = string.Empty;
        public string? TimeSlot { get; set; } = string.Empty;
        public string? Slot1 { get; set; } = string.Empty;
        public string? Slot2 { get; set; } = string.Empty;
        public string? Room { get; set; } = string.Empty;
        public string? Status { get; set; } = string.Empty;
        public string? Lecturer { get; set; } = string.Empty;


        public ExportInImportFormatDTO(string? className, string? subjectName, string? department, string? timeSlot, string? slot1, string? slot2, string? room, string? status, string? lecturer)
        {
            Class = className;
            Subject = subjectName;
            Dept = department;
            TimeSlot = timeSlot;
            Slot1 = slot1;
            Slot2 = slot2;
            Room = room;
            Status = status;
            Lecturer = lecturer;
        }

        public ExportInImportFormatDTO()
        {
        }
    }
}
