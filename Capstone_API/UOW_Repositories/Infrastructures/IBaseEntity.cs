using Capstone_API.Enum;

namespace Capstone_API.UOW_Repositories.Infrastructures
{
    public interface IBaseEntity
    {
        int Id { get; set; }
        Status ExistStatus { get; set; }
        DateTime CreateOn { get; set; }
        DateTime UpdateOn { get; set; }
    }
}
