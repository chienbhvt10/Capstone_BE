using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SemesterInfo
    {
        public SemesterInfo()
        {
            Buildings = new HashSet<Building>();
            SlotPreferenceLevels = new HashSet<SlotPreferenceLevel>();
            SubjectPreferenceLevels = new HashSet<SubjectPreferenceLevel>();
            Subjects = new HashSet<Subject>();
            TaskAssigns = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public bool? IsNow { get; set; }
        public string? Semester { get; set; }
        public int? Year { get; set; }
        public int? DepartmentHeadId { get; set; }

        public virtual User? DepartmentHead { get; set; }
        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<SlotPreferenceLevel> SlotPreferenceLevels { get; set; }
        public virtual ICollection<SubjectPreferenceLevel> SubjectPreferenceLevels { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
    }
}
