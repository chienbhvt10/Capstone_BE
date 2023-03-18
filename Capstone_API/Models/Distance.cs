using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class Distance
    {
        public int Id { get; set; }
        public int? Building1Id { get; set; }
        public int? Building2Id { get; set; }
        public int? DistanceBetween { get; set; }
        public int? SemesterId { get; set; }

        public virtual Building? Building1 { get; set; }
        public virtual Building? Building2 { get; set; }
    }
}
