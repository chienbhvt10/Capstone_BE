using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(Class entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(Class entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<Class, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<Class, bool> condition, bool isHardDeleted = false);
    }
}
