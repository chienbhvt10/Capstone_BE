using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class ExecuteInfoRepository : GenericRepository<ExecuteInfo>, IExecuteInfoRepository
    {
        public ExecuteInfoRepository(CapstoneDataContext context) : base(context)
        {
        }

    }
}
