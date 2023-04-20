using Capstone_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Capstone_API.UOW_Repositories.Infrastructures
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
       where TEntity : class
    {

        protected readonly CapstoneDataContext Context;

        private readonly DbSet<TEntity> _dbSet;

        protected GenericRepository(CapstoneDataContext context)
        {
            Context = context;
            _dbSet = context.Set<TEntity>();
        }

        #region Gets

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _dbSet;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual TEntity GetById(params object[] keyValues)
        {
            return _dbSet.Find(keyValues);
        }

        public virtual async Task<TEntity> GetByIdAsync(params object[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues);
        }

        public virtual IEnumerable<TEntity> GetByCondition(Func<TEntity, bool> condition)
        {
            return _dbSet.Where(condition);
        }

        #endregion

        #region Finds


        public virtual TEntity Find(object entityId)
        {
            return _dbSet.Find(entityId);
        }


        public virtual TEntity Find(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }


        public virtual async Task<TEntity> FindAsync(params object[] entityIds)
        {
            return await _dbSet.FindAsync(entityIds);
        }


        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }


        public virtual IEnumerable<TEntity> Finds(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        #endregion

        #region Adds


        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }


        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }


        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Updates

        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
        }
        #endregion



    }
}
