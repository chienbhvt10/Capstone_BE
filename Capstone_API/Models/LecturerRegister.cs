using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class LecturerRegister
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
        public int SlotId { get; set; }
        public string? Note { get; set; }
        public int SemesterId { get; set; }
        public int? TimeSlotId { get; set; }

        public virtual Lecturer Lecturer { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual TimeSlot? TimeSlot { get; set; }
    }
}
