using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class User
    {
        public User()
        {
            Buildings = new HashSet<Building>();
            ExecuteInfos = new HashSet<ExecuteInfo>();
            Lecturers = new HashSet<Lecturer>();
            SemesterInfos = new HashSet<SemesterInfo>();
            TaskAssigns = new HashSet<TaskAssign>();
            TimeSlots = new HashSet<TimeSlot>();
        }

        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department? Department { get; set; }
        public virtual ICollection<Building> Buildings { get; set; }
        public virtual ICollection<ExecuteInfo> ExecuteInfos { get; set; }
        public virtual ICollection<Lecturer> Lecturers { get; set; }
        public virtual ICollection<SemesterInfo> SemesterInfos { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
        public virtual ICollection<TimeSlot> TimeSlots { get; set; }
    }
}
