using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class SlotPreferenceLevelRepository : GenericRepository<SlotPreferenceLevel>, ISlotPreferenceLevelRepository
    {
        public SlotPreferenceLevelRepository(CapstoneDataContext context) : base(context)
        {
        }
    }
}
