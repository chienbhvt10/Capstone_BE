using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TimeSlotSegment
    {
        public int Id { get; set; }
        public int? DayOfWeek { get; set; }
        public int? SlotInDay { get; set; }
        public int? SemesterId { get; set; }

        public virtual TimeSlot? SlotInDayNavigation { get; set; }
    }
}
