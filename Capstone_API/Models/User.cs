using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? DepartmentId { get; set; }
    }
}
