using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SlotDay
    {
        public int Id { get; set; }
        public int? NumberOfSlots { get; set; }
        public int? SemesterId { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string? ExistStatus { get; set; }
    }
}
