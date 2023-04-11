using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Department
    {
        public Department()
        {
            Lecturers = new HashSet<Lecturer>();
        }

        public int Id { get; set; }
        public string? Department1 { get; set; }

        public virtual ICollection<Lecturer> Lecturers { get; set; }
    }
}
