using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TaskAssign
    {
        public int Id { get; set; }
        public int? LecturerId { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public int? TimeSlotId { get; set; }
        public int? Room1Id { get; set; }
        public bool? PreAssign { get; set; }
        public bool? Status { get; set; }
        public int? DepartmentHeadId { get; set; }
        public int? SemesterId { get; set; }

        public virtual Class? Class { get; set; }
        public virtual User? DepartmentHead { get; set; }
        public virtual Lecturer? Lecturer { get; set; }
        public virtual Room? Room1 { get; set; }
        public virtual SemesterInfo? Semester { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
    }
}
