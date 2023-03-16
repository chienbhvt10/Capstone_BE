using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ITimeSlotCompatibilityRepository : IGenericRepository<TimeSlotCompatibility>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(TimeSlotCompatibility entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(TimeSlotCompatibility entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<TimeSlotCompatibility, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<TimeSlotCompatibility, bool> condition, bool isHardDeleted = false);
    }
}
