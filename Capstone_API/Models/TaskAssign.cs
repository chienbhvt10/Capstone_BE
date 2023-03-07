using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class TaskAssign
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public int? SubjectId { get; set; }
        public string? Department { get; set; }
        public int? TimeSlotId { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
        public int? SemesterId { get; set; }
        public bool? Status { get; set; }
        public int? LecturerId { get; set; }
        public int? Room1Id { get; set; }
        public int? Room2Id { get; set; }
        public string? ExistStatus { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }

        public virtual Class? Class { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual TimeSlot? TimeSlot { get; set; }
    }
}
