using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SlotPreferenceLevel
    {
        public int Id { get; set; }
        public int? LecturerId { get; set; }
        public int? SlotId { get; set; }
        public int? PreferenceLevel { get; set; }
        public int? DepartmentHeadId { get; set; }
        public int? SemesterId { get; set; }

        public virtual Lecturer? Lecturer { get; set; }
        public virtual TimeSlot? Slot { get; set; }
    }
}
