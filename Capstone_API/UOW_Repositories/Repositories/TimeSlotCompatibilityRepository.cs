using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class TimeSlotCompatibilityRepository : GenericRepository<TimeSlotCompatibility>, ITimeSlotCompatibilityRepository
    {
        public TimeSlotCompatibilityRepository(CapstoneDataContext context) : base(context)
        {
        }
    }
}
