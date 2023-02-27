using Capstone_API.Enum;

namespace Capstone_API.UOW_Repositories.Infrastructures
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
        public Status ExistStatus { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }
    }
}
