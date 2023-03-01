using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Semester
    {
        public Semester()
        {
            SlotPreferenceLevels = new HashSet<SlotPreferenceLevel>();
            SubjectPreferenceLevels = new HashSet<SubjectPreferenceLevel>();
            Subjects = new HashSet<Subject>();
            TaskAssigns = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Year { get; set; }

        public virtual ICollection<SlotPreferenceLevel> SlotPreferenceLevels { get; set; }
        public virtual ICollection<SubjectPreferenceLevel> SubjectPreferenceLevels { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
    }
}
