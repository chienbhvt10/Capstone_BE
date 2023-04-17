using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Room
    {
        public Room()
        {
            TaskAssigns = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public int? DepartmentHeadId { get; set; }
        public int? SemesterId { get; set; }

        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
    }
}
