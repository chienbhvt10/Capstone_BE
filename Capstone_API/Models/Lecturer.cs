using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.Models
{
    public partial class Lecturer : BaseEntity
    {
        public Lecturer()
        {
            LecturerRegisters = new HashSet<LecturerRegister>();
            SlotPreferenceLevels = new HashSet<SlotPreferenceLevel>();
            SubjectPreferenceLevels = new HashSet<SubjectPreferenceLevel>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }
        public int? SemesterId { get; set; }
        public int? OrderNumber { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? UpdateOn { get; set; }
        public string? ExistStatus { get; set; }

        public virtual ICollection<LecturerRegister> LecturerRegisters { get; set; }
        public virtual ICollection<SlotPreferenceLevel> SlotPreferenceLevels { get; set; }
        public virtual ICollection<SubjectPreferenceLevel> SubjectPreferenceLevels { get; set; }
    }
}
