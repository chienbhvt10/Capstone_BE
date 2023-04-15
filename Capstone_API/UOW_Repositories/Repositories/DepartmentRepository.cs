using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Capstone_API.UOW_Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        CapstoneDataContext _context;
        public DepartmentRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }

        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.Departments.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(Department)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.Departments.Remove(entity);
        }

        public virtual void Delete(Department entity, bool isHardDeleted = false)
        {
            var entityExist = _context.Departments.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(Department)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.Departments.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.Departments.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(Department)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.Departments.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(Department entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.Departments.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(Department)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.Departments.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.Departments.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(Department)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.Departments.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<Department, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.Departments.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<Department, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.Departments.Where(condition).ToList();
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }
    }
}
