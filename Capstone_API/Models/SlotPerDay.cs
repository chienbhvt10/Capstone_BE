﻿using System;
using System.Collections.Generic;

namespace Capstone_API.Models
{
    public partial class SlotPerDay
    {
        public int Id { get; set; }
        public int? NumberOfSlots { get; set; }
        public int? SemesterId { get; set; }
    }
}
