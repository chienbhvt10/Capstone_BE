using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            LecturerRegisters = new HashSet<LecturerRegister>();
            SlotPreferenceLevels = new HashSet<SlotPreferenceLevel>();
            TaskAssigns = new HashSet<TaskAssign>();
            TimeSlotCompatibilities = new HashSet<TimeSlotCompatibility>();
            TimeSlotConflicts = new HashSet<TimeSlotConflict>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SemesterId { get; set; }
        public int? OrderNumber { get; set; }

        public virtual ICollection<LecturerRegister> LecturerRegisters { get; set; }
        public virtual ICollection<SlotPreferenceLevel> SlotPreferenceLevels { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
        public virtual ICollection<TimeSlotCompatibility> TimeSlotCompatibilities { get; set; }
        public virtual ICollection<TimeSlotConflict> TimeSlotConflicts { get; set; }
    }
}
