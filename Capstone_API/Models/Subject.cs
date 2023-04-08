using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Subject
    {
        public Subject()
        {
            SubjectPreferenceLevels = new HashSet<SubjectPreferenceLevel>();
            TaskAssigns = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? SemesterId { get; set; }
        public string? Department { get; set; }

        public virtual Semester? Semester { get; set; }
        public virtual ICollection<SubjectPreferenceLevel> SubjectPreferenceLevels { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
    }
}
