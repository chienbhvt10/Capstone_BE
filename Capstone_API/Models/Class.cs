using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Class
    {
        public Class()
        {
            TaskAssigns = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
    }
}
