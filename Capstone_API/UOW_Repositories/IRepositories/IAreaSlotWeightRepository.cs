using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface IAreaSlotWeightRepository : IGenericRepository<AreaSlotWeight>
    {
        IEnumerable<AreaSlotWeight> TimeSlotData();
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(AreaSlotWeight entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(AreaSlotWeight entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<AreaSlotWeight, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<AreaSlotWeight, bool> condition, bool isHardDeleted = false);
    }
}
