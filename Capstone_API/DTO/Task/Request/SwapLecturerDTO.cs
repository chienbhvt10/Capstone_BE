﻿namespace Capstone_API.DTO.Task.Request
{
    public class SwapLecturerDTO
    {
        public int LecturerId { get; set; }
        public int TaskId { get; set; }
        public int TimeSlotId { get; set; }
        public int SemesterId { get; set; }
        public int DepartmentHeadId { get; set; }
    }
}
