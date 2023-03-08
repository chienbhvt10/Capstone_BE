using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ITaskRepository : IGenericRepository<TaskAssign>
    {
        IEnumerable<TaskAssign> MappingTaskData();
    }
}
