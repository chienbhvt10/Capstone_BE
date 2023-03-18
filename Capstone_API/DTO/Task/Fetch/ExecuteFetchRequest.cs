﻿namespace Capstone_API.DTO.Task.Fetch
{
    public class ExecuteFetchRequest
    {
        public SettingRequest? Setting { get; set; }
        public List<TaskFetchRequest>? Tasks { get; set; }
        public List<SlotFetchRequest>? Slots { get; set; }
        public List<InstructorFetchRequest>? Instructors { get; set; }
        public int NumTasks { get; set; }
        public int NumInstructors { get; set; }
        public int NumSlots { get; set; }
        public int NumSubjects { get; set; }
        public int NumAreas { get; set; }
        public int BackupInstructor { get; set; }
        public List<List<int>>? SlotConflict { get; set; }
        public List<List<int?>>? SlotCompability { get; set; }
        public List<List<int?>>? InstructorSubject { get; set; }
        public List<List<int?>>? InstructorSlot { get; set; }
        public List<int?>? InstructorQuota { get; set; }
        public List<List<int?>>? AreaDistance { get; set; }
        public List<List<int?>>? AreaSlotCoefficient { get; set; }
        public List<TaskPreAssignFetchRequest>? PreAssign { get; set; }
    }

    public class SlotFetchRequest
    {
        public string? Id { get; set; }
        public int Order { get; set; }
    }
    public class InstructorFetchRequest
    {
        public string? Id { get; set; }
        public int Order { get; set; }
    }

    public class SettingRequest
    {
        public int MaxSearchingTime { get; set; }
        public int Solver { get; set; }
        public int Strategy { get; set; }
        public List<int>? ObjectiveOption { get; set; }
        public List<int>? ObjectiveWeight { get; set; }

    }

    public class TaskFetchRequest
    {
        public string? Id { get; set; }
        public int Order { get; set; }
        public int SlotOrder { get; set; }
        public int SubjectOrder { get; set; }
        public int AreaOrder { get; set; }
    }

    public class TaskPreAssignFetchRequest
    {
        public int InstructorOrder { get; set; }
        public int TaskOrder { get; set; }
        public int Type { get; set; }

    }

    public class SubjectFetchRequest
    {
        public string? Id { get; set; }
        public int Order { get; set; }
    }
}