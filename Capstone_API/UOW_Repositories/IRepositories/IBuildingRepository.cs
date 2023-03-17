using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface IBuildingRepository : IGenericRepository<Building>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(Building entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(Building entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<Building, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<Building, bool> condition, bool isHardDeleted = false);
    }
}
