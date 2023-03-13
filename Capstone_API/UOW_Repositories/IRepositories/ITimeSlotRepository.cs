using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ITimeSlotRepository : IGenericRepository<TimeSlot>
    {
        IEnumerable<TimeSlot> MappingTimeSlotData();
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(TimeSlot entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(TimeSlot entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<TimeSlot, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<TimeSlot, bool> condition, bool isHardDeleted = false);
    }
}
