using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class LecturerQuotaRepository : GenericRepository<LecturerQuotum>, ILecturerQuotaRepository
    {
        CapstoneDataContext _context;
        public LecturerQuotaRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }

        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.LecturerQuota.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(LecturerQuotum)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.LecturerQuota.Remove(entity);
        }

        public virtual void Delete(LecturerQuotum entity, bool isHardDeleted = false)
        {
            var entityExist = _context.LecturerQuota.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(LecturerQuotum)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.LecturerQuota.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.LecturerQuota.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(LecturerQuotum)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.LecturerQuota.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(LecturerQuotum entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.LecturerQuota.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(LecturerQuotum)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.LecturerQuota.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.LecturerQuota.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(LecturerQuotum)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.LecturerQuota.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<LecturerQuotum, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.LecturerQuota.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<LecturerQuotum, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.LecturerQuota.Where(condition).ToList();
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }
    }
}
