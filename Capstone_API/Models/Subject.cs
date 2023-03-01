using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.Models
{
    public partial class Subject : BaseEntity
    {
        public Subject()
        {
            LecturerRegisters = new HashSet<LecturerRegister>();
            SubjectPreferenceLevels = new HashSet<SubjectPreferenceLevel>();
            TaskAssigns = new HashSet<TaskAssign>();
        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? SemesterId { get; set; }
        public int? OrderNumber { get; set; }
        public string? Department { get; set; }

        public virtual Semester? Semester { get; set; }
        public virtual ICollection<LecturerRegister> LecturerRegisters { get; set; }
        public virtual ICollection<SubjectPreferenceLevel> SubjectPreferenceLevels { get; set; }
        public virtual ICollection<TaskAssign> TaskAssigns { get; set; }
    }
}
