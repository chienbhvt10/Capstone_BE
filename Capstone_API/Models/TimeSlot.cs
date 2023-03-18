﻿using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            AreaSlotWeightAreaSlots = new HashSet<AreaSlotWeight>();
            AreaSlotWeightSlots = new HashSet<AreaSlotWeight>();
            LecturerRegisters = new HashSet<LecturerRegister>();
            SlotPreferenceLevels = new HashSet<SlotPreferenceLevel>();
            TaskAssigns = new HashSet<TaskAssign>();
            TimeSlotCompatibilityCompatibilitySlots = new HashSet<TimeSlotCompatibility>();
            TimeSlotCompatibilitySlots = new HashSet<TimeSlotCompatibility>();
            TimeSlotConflictConflictSlots = new HashSet<TimeSlotConflict>();
            TimeSlotConflictSlots = new HashSet<TimeSlotConflict>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SemesterId { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }

        public virtual ICollection<AreaSlotWeight> AreaSlotWeightAreaSlots { get; set; }
        public virtual ICollection<AreaSlotWeight> AreaSlotWeightSlots { get; set; }
        public virtual ICollection<LecturerRegister> LecturerRegisters { get; set; }
        public virtual ICollection<SlotPreferenceLevel> SlotPreferenceLevels { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
        public virtual ICollection<TimeSlotCompatibility> TimeSlotCompatibilityCompatibilitySlots { get; set; }
        public virtual ICollection<TimeSlotCompatibility> TimeSlotCompatibilitySlots { get; set; }
        public virtual ICollection<TimeSlotConflict> TimeSlotConflictConflictSlots { get; set; }
        public virtual ICollection<TimeSlotConflict> TimeSlotConflictSlots { get; set; }
    }
}
