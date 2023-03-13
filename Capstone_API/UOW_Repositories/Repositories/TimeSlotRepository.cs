using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class TimeSlotRepository : GenericRepository<TimeSlot>, ITimeSlotRepository
    {
        CapstoneDataContext _context;
        public TimeSlotRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<TimeSlot> MappingTimeSlotData()
        {
            var items = _context.TimeSlots
                .Include(ts => ts.TaskAssigns)
                    .ThenInclude(task => task.Lecturer)
                .Select(item => item);
            return items;
        }
        #region Delete
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.TimeSlots.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(TimeSlot)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TimeSlots.Remove(entity);
        }

        public virtual void Delete(TimeSlot entity, bool isHardDeleted = false)
        {
            var entityExist = _context.TimeSlots.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TimeSlot)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TimeSlots.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.TimeSlots.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(TimeSlot)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.TimeSlots.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(TimeSlot entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.TimeSlots.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TimeSlot)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TimeSlots.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.TimeSlots.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(TimeSlot)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TimeSlots.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<TimeSlot, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TimeSlots.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<TimeSlot, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TimeSlots.Where(condition);
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }

        #endregion
    }
}
