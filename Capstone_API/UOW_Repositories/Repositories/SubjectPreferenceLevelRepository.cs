using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class SubjectPreferenceLevelRepository : GenericRepository<SubjectPreferenceLevel>, ISubjectPreferenceLevelRepository
    {
        CapstoneDataContext _context;
        public SubjectPreferenceLevelRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.SubjectPreferenceLevels.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(SubjectPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.SubjectPreferenceLevels.Remove(entity);
        }

        public virtual void Delete(SubjectPreferenceLevel entity, bool isHardDeleted = false)
        {
            var entityExist = _context.SubjectPreferenceLevels.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(SubjectPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.SubjectPreferenceLevels.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.SubjectPreferenceLevels.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(SubjectPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.SubjectPreferenceLevels.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(SubjectPreferenceLevel entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.SubjectPreferenceLevels.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(SubjectPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.SubjectPreferenceLevels.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.SubjectPreferenceLevels.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(SubjectPreferenceLevel)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.SubjectPreferenceLevels.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<SubjectPreferenceLevel, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.SubjectPreferenceLevels.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<SubjectPreferenceLevel, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.SubjectPreferenceLevels.Where(condition).ToList();
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }
    }
}
