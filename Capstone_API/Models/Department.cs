using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Department
    {
        public Department()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }
        public string? Department1 { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
