using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class SubjectPreferenceLevelRepository : GenericRepository<SubjectPreferenceLevel>, ISubjectPreferenceLevelRepository
    {
        public SubjectPreferenceLevelRepository(CapstoneDataContext context) : base(context)
        {
        }
    }
}
