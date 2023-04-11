using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        CapstoneDataContext _context;
        public SemesterRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }

        #region Delete
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.Semesters.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(Semester)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.Semesters.Remove(entity);
        }

        public virtual void Delete(Semester entity, bool isHardDeleted = false)
        {
            var entityExist = _context.Semesters.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(Semester)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.Semesters.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.Semesters.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(Semester)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.Semesters.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(Semester entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.Semesters.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(Semester)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.Semesters.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.Semesters.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(Semester)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.Semesters.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<Semester, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.Semesters.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<Semester, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.Semesters.Where(condition);
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }

        #endregion
    }
}
