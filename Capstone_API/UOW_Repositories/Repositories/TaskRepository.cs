using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using System.Linq.Expressions;

namespace Capstone_API.UOW_Repositories.Repositories
{
    public class TaskRepository : GenericRepository<TaskAssign>, ITaskRepository
    {
        public TaskRepository(CapstoneDataContext context) : base(context)
        {
        }

        public void Add(TaskAssign entity)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(TaskAssign entity)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TaskAssign> entities)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<TaskAssign> entities)
        {
            throw new NotImplementedException();
        }

        public int Count(bool isHardDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(bool isHardDeleted = false)
        {
            throw new NotImplementedException();
        }

        public void Delete(int entityId, bool isHardDeleted = false)
        {
            throw new NotImplementedException();
        }

        public void Delete(TaskAssign entity, bool isHardDeleted = false)
        {
            throw new NotImplementedException();
        }

        public void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TaskAssign entity, bool isHardDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void DeleteByCondition(Func<TaskAssign, bool> condition, bool isHardDeleted = false)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByConditionAsync(Func<TaskAssign, bool> condition, bool isHardDeleted = false)
        {
            throw new NotImplementedException();
        }

        public TaskAssign Find(object entityId)
        {
            throw new NotImplementedException();
        }

        public Task<TaskAssign> FindAsync(params object[] entityIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskAssign> Finds(Expression<Func<TaskAssign, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskAssign> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskAssign>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskAssign> GetByCondition(Func<TaskAssign, bool> condition)
        {
            throw new NotImplementedException();
        }

        public TaskAssign GetById(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public Task<TaskAssign> GetByIdAsync(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void Update(TaskAssign entity)
        {
            throw new NotImplementedException();
        }
    }
}
