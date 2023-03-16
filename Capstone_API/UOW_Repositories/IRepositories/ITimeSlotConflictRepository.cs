using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ITimeSlotConflictRepository : IGenericRepository<TimeSlotConflict>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(TimeSlotConflict entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(TimeSlotConflict entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<TimeSlotConflict, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<TimeSlotConflict, bool> condition, bool isHardDeleted = false);
    }
}
