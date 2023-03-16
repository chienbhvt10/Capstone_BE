using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class LecturerQuotum
    {
        public int Id { get; set; }
        public int? LecturerId { get; set; }
        public int? Quota { get; set; }
        public int? SemesterId { get; set; }

        public virtual Lecturer? Lecturer { get; set; }
        public virtual Semester? Semester { get; set; }
    }
}
