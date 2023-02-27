﻿using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SubjectPreferenceLevel
    {
        public int Id { get; set; }
        public int LecturerId { get; set; }
        public int SubjectId { get; set; }
        public int PreferenceLevel { get; set; }
        public int SemesterId { get; set; }

        public virtual Lecturer Lecturer { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
    }
}
