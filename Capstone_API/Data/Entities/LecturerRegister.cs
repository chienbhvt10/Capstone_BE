namespace Capstone_API.Data.Entities
{
    public class LecturerRegister
    {

        public int Id { get; set; }
        public int Lecturer_id { get; set; }
        public int Subject_id { get; set; }
        public int Slot_id { get; set; }
        public int Note { get; set; }
        public int SemesterId { get; set; }

        public LecturerRegister(int id, int lecturer_id, int subject_id, int slot_id, int note, int semesterId)
        {
            Id = id;
            Lecturer_id = lecturer_id;
            Subject_id = subject_id;
            Slot_id = slot_id;
            Note = note;
            SemesterId = semesterId;
        }

        public LecturerRegister()
        {
        }
    }
}
