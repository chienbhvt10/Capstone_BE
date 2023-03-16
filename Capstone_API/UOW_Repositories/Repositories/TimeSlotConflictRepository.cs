using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class TimeSlotConflictRepository : GenericRepository<TimeSlotConflict>, ITimeSlotConflictRepository
    {
        CapstoneDataContext _context;
        public TimeSlotConflictRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.TimeSlotConflicts.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(TimeSlotConflict)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotConflicts.Remove(entity);
        }

        public virtual void Delete(TimeSlotConflict entity, bool isHardDeleted = false)
        {
            var entityExist = _context.TimeSlotConflicts.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TimeSlotConflict)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotConflicts.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.TimeSlotConflicts.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(TimeSlotConflict)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotConflicts.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(TimeSlotConflict entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.TimeSlotConflicts.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TimeSlotConflict)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TimeSlotConflicts.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.TimeSlotConflicts.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(TimeSlotConflict)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TimeSlotConflicts.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<TimeSlotConflict, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TimeSlotConflicts.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<TimeSlotConflict, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TimeSlotConflicts.Where(condition).ToList();
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }
    }
}
