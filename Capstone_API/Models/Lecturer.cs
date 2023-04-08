using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Lecturer
    {
        public Lecturer()
        {
            SlotPreferenceLevels = new HashSet<SlotPreferenceLevel>();
            SubjectPreferenceLevels = new HashSet<SubjectPreferenceLevel>();
            TaskAssigns = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public string? Email { get; set; }
        public int? Quota { get; set; }
        public int? MinQuota { get; set; }
        public int? SemesterId { get; set; }

        public virtual ICollection<SlotPreferenceLevel> SlotPreferenceLevels { get; set; }
        public virtual ICollection<SubjectPreferenceLevel> SubjectPreferenceLevels { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
    }
}
