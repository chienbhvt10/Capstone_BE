using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.Models
{
    public partial class TaskAssign : BaseEntity
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public string? Department { get; set; }
        public int TimeSlotId { get; set; }
        public string? Slot1 { get; set; }
        public string? Slot2 { get; set; }
        public int SemesterId { get; set; }
        public bool Status { get; set; }

        public virtual Class Class { get; set; } = null!;
        public virtual Semester Semester { get; set; } = null!;
        public virtual Subject Subject { get; set; } = null!;
        public virtual TimeSlot TimeSlot { get; set; } = null!;
    }
}
