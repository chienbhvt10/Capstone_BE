using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SemesterInfo
    {
        public int Id { get; set; }
        public bool? IsNow { get; set; }
        public string? Semester { get; set; }
        public int? Year { get; set; }
        public int? DepartmentHeadId { get; set; }

        public virtual User? DepartmentHead { get; set; }
    }
}
