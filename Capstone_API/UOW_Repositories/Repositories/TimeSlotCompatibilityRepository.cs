using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class TimeSlotCompatibilityRepository : GenericRepository<TimeSlotCompatibility>, ITimeSlotCompatibilityRepository
    {
        CapstoneDataContext _context;
        public TimeSlotCompatibilityRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<TimeSlotCompatibility> TimeSlotData()
        {
            var items = _context.TimeSlotCompatibilities
                .Include(item => item.CompatibilitySlot);
            return items;
        }

        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.TimeSlotCompatibilities.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(TimeSlotCompatibility)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotCompatibilities.Remove(entity);
        }

        public virtual void Delete(TimeSlotCompatibility entity, bool isHardDeleted = false)
        {
            var entityExist = _context.TimeSlotCompatibilities.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TimeSlotCompatibility)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotCompatibilities.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.TimeSlotCompatibilities.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(TimeSlotCompatibility)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotCompatibilities.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(TimeSlotCompatibility entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.TimeSlotCompatibilities.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TimeSlotCompatibility)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TimeSlotCompatibilities.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.TimeSlotCompatibilities.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(TimeSlotCompatibility)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TimeSlotCompatibilities.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<TimeSlotCompatibility, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TimeSlotCompatibilities.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<TimeSlotCompatibility, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TimeSlotCompatibilities.Where(condition).ToList();
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }
    }
}
