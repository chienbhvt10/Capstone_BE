namespace Capstone_API.Data.Entities
{
    public class LecturerRegister
    {

        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
        public int SlotId { get; set; }
        public string? Note { get; set; }
        public int SemesterId { get; set; }

        public LecturerRegister(int id, int lecturer_id, int subject_id, int slot_id, string note, int semesterId)
        {
            Id = id;
            LecturerId = lecturer_id;
            SubjectId = subject_id;
            SlotId = slot_id;
            Note = note;
            SemesterId = semesterId;
        }

        public LecturerRegister()
        {
        }
    }
}
