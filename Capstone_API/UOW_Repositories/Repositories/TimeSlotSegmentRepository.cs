using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class TimeSlotSegmentRepository : GenericRepository<TimeSlotSegment>, ITimeSlotSegmentRepository
    {
        CapstoneDataContext _context;
        public TimeSlotSegmentRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.TimeSlotSegments.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(TimeSlotSegment)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotSegments.Remove(entity);
        }

        public virtual void Delete(TimeSlotSegment entity, bool isHardDeleted = false)
        {
            var entityExist = _context.TimeSlotSegments.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TimeSlotSegment)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotSegments.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.TimeSlotSegments.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(TimeSlotSegment)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.TimeSlotSegments.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(TimeSlotSegment entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.TimeSlotSegments.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TimeSlotSegment)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TimeSlotSegments.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.TimeSlotSegments.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(TimeSlotSegment)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TimeSlotSegments.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<TimeSlotSegment, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TimeSlotSegments.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<TimeSlotSegment, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TimeSlotSegments.Where(condition).ToList();
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }
    }
}
