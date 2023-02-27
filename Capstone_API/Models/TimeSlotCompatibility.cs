using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TimeSlotCompatibility
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public int CompatibilitySlotId { get; set; }
        public int CompatibilityLevel { get; set; }
        public int SemesterId { get; set; }
        public int? TimeSlotId { get; set; }

        public virtual TimeSlot? TimeSlot { get; set; }
    }
}
