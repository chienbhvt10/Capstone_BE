using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(User entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(User entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<User, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<User, bool> condition, bool isHardDeleted = false);
    }
}
