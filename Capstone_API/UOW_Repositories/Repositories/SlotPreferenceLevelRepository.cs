using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class SlotPreferenceLevelRepository : GenericRepository<SlotPreferenceLevel>, ISlotPreferenceLevelRepository
    {
        CapstoneDataContext _context;
        public SlotPreferenceLevelRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<SlotPreferenceLevel> MappingTaskData()
        {
            var items = _context.SlotPreferenceLevels
                .Include(item => item.Lecturer)
                .Include(item => item.Slot);
            return items;
        }
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.SlotPreferenceLevels.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(SlotPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.SlotPreferenceLevels.Remove(entity);
        }

        public virtual void Delete(SlotPreferenceLevel entity, bool isHardDeleted = false)
        {
            var entityExist = _context.SlotPreferenceLevels.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(SlotPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.SlotPreferenceLevels.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.SlotPreferenceLevels.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(SlotPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.SlotPreferenceLevels.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(SlotPreferenceLevel entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.SlotPreferenceLevels.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(SlotPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.SlotPreferenceLevels.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.SlotPreferenceLevels.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(SlotPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.SlotPreferenceLevels.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<SlotPreferenceLevel, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.SlotPreferenceLevels.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<SlotPreferenceLevel, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.SlotPreferenceLevels.Where(condition).ToList();
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }
    }
}
