using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Room
    {
        public Room()
        {
            TaskAssignRoom1s = new HashSet<TaskAssign>();
            TaskAssignRoom2s = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? SemesterId { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string? ExistStatus { get; set; }

        public virtual ICollection<TaskAssign> TaskAssignRoom1s { get; set; }
        public virtual ICollection<TaskAssign> TaskAssignRoom2s { get; set; }
    }
}
