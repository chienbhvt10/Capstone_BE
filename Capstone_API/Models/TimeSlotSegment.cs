using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TimeSlotSegment
    {
        public int Id { get; set; }
        public int? SlotId { get; set; }
        public int? DayOfWeek { get; set; }
        public int? Segment { get; set; }
        public int? SemesterId { get; set; }

        public virtual DayOfWeek? DayOfWeekNavigation { get; set; }
        public virtual TimeSlot? Slot { get; set; }
    }
}
