using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(Room entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(Room entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<Room, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<Room, bool> condition, bool isHardDeleted = false);
    }
}
