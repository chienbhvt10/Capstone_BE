using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class AreaSlotWeight
    {
        public int Id { get; set; }
        public int? SlotId { get; set; }
        public int? AreaSlotId { get; set; }
        public int? AreaSlotWeight1 { get; set; }
        public int? DepartmentHeadId { get; set; }
        public int? SemesterId { get; set; }

        public virtual TimeSlot? AreaSlot { get; set; }
        public virtual TimeSlot? Slot { get; set; }
    }
}
