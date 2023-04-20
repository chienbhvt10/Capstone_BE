using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class DayOfWeek
    {
        public DayOfWeek()
        {
            TimeSlotSegments = new HashSet<TimeSlotSegment>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TimeSlotSegment> TimeSlotSegments { get; set; }
    }
}
