using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ITaskRepository : IGenericRepository<TaskAssign>
    {
        IEnumerable<TaskAssign> MappingTaskData();
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(TaskAssign entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(TaskAssign entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<TaskAssign, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<TaskAssign, bool> condition, bool isHardDeleted = false);
    }
}
