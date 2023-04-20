using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class SemesterInfoRepository : GenericRepository<SemesterInfo>, ISemesterInfoRepository
    {
        CapstoneDataContext _context;
        public SemesterInfoRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }

        #region Delete
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.SemesterInfos.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(SemesterInfo)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.SemesterInfos.Remove(entity);
        }

        public virtual void Delete(SemesterInfo entity, bool isHardDeleted = false)
        {
            var entityExist = _context.SemesterInfos.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(SemesterInfo)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.SemesterInfos.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.SemesterInfos.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(SemesterInfo)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.SemesterInfos.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(SemesterInfo entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.SemesterInfos.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(SemesterInfo)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.SemesterInfos.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.SemesterInfos.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(SemesterInfo)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.SemesterInfos.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<SemesterInfo, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.SemesterInfos.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<SemesterInfo, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.SemesterInfos.Where(condition);
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }

        #endregion
    }
}
