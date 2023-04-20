using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TimeSlotConflict
    {
        public int Id { get; set; }
        public int? SlotId { get; set; }
        public int? ConflictSlotId { get; set; }
        public bool? Conflict { get; set; }
        public int? DepartmentHeadId { get; set; }
        public int? SemesterId { get; set; }

        public virtual TimeSlot? ConflictSlot { get; set; }
        public virtual TimeSlot? Slot { get; set; }
    }
}
