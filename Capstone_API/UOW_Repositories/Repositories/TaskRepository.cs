using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public class TaskRepository : GenericRepository<TaskAssign>, ITaskRepository
    {
        CapstoneDataContext _context;
        public TaskRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<TaskAssign> MappingTaskData()
        {
            var items = _context.TaskAssigns
                .Include(task => task.Class)
                .Include(task => task.Subject)
                .Include(task => task.Lecturer)
                .Include(task => task.Room1)
                .Include(task => task.TimeSlot).Select(item => item);
            return items;
        }

        #region Delete
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _context.TaskAssigns.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(TaskAssign)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TaskAssigns.Remove(entity);
        }

        public virtual void Delete(TaskAssign entity, bool isHardDeleted = false)
        {
            var entityExist = _context.TaskAssigns.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TaskAssign)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _context.TaskAssigns.Remove(entity);
        }

        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _context.TaskAssigns.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(TaskAssign)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _context.TaskAssigns.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(TaskAssign entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _context.TaskAssigns.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TaskAssign)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TaskAssigns.Remove(entitiesExist);
        }

        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _context.TaskAssigns.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(TaskAssign)}");

            if (isHardDeleted == false)
            {
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _context.TaskAssigns.Remove(entitiesExist);
        }

        public virtual void DeleteByCondition(Func<TaskAssign, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TaskAssigns.Where(condition).ToList();
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        public virtual async Task DeleteByConditionAsync(Func<TaskAssign, bool> condition, bool isHardDeleted = false)
        {
            var query = _context.TaskAssigns.Where(condition);
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }

        #endregion
    }
}
