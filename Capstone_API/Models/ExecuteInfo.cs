using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class ExecuteInfo
    {
        public int Id { get; set; }
        public int? ExecuteId { get; set; }
        public DateTime? ExecuteTime { get; set; }
        public int? SemesterId { get; set; }
    }
}
