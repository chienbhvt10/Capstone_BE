using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public class TaskRepository : GenericRepository<TaskAssign>, ITaskRepository
    {
        public TaskRepository(CapstoneDataContext context) : base(context)
        {
        }
    }
}
