using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Building
    {
        public Building()
        {
            DistanceBuilding1s = new HashSet<Distance>();
            DistanceBuilding2s = new HashSet<Distance>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int? DepartmentHeadId { get; set; }
        public int? SemesterId { get; set; }

        public virtual User? DepartmentHead { get; set; }
        public virtual SemesterInfo? Semester { get; set; }
        public virtual ICollection<Distance> DistanceBuilding1s { get; set; }
        public virtual ICollection<Distance> DistanceBuilding2s { get; set; }
    }
}
