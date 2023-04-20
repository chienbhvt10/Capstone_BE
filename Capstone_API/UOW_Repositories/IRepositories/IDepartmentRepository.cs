using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(Department entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(Department entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<Department, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<Department, bool> condition, bool isHardDeleted = false);
    }
}
