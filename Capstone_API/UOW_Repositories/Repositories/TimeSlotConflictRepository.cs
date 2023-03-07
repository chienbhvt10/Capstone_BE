using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class TimeSlotConflictRepository : GenericRepository<TimeSlotConflict>, ITimeSlotConflictRepository
    {
        public TimeSlotConflictRepository(CapstoneDataContext context) : base(context)
        {
        }
    }
}
