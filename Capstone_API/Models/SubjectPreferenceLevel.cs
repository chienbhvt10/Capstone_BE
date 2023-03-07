using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SubjectPreferenceLevel
    {
        public int Id { get; set; }
        public int? LecturerId { get; set; }
        public int? SubjectId { get; set; }
        public int? PreferenceLevel { get; set; }
        public int? SemesterId { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string? ExistStatus { get; set; }

        public virtual Lecturer? Lecturer { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Subject? Subject { get; set; }
    }
}
