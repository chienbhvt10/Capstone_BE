using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SlotPreferenceLevel
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int SlotId { get; set; }
        public int PreferenceLevel { get; set; }
        public int SemesterId { get; set; }
        public int? TimeSlotInstanceId { get; set; }

        public virtual Lecturer Lecturer { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;
        public virtual TimeSlot? TimeSlotInstance { get; set; }
    }
}
