using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TimeSlotConflict
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public int ConflictSlotId { get; set; }
        public bool Conflict { get; set; }
        public int SemesterId { get; set; }
        public int? TimeSlotId { get; set; }

        public virtual TimeSlot? TimeSlot { get; set; }
    }
}
