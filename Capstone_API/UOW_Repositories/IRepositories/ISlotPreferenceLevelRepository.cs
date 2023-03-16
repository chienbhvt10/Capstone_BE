using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ISlotPreferenceLevelRepository : IGenericRepository<SlotPreferenceLevel>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(SlotPreferenceLevel entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(SlotPreferenceLevel entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<SlotPreferenceLevel, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<SlotPreferenceLevel, bool> condition, bool isHardDeleted = false);
    }
}
