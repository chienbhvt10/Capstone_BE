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

        //// Get Options
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

        //// Find Options

        /// <summary>
        /// This method use to find by entity id 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>return an Entity exist</returns>
        public virtual TEntity Find(object entityId)
        {
            return _dbSet.Find(entityId);
        }

        /// <summary>
        /// This method use to find by condition
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>return an Entity exist</returns>
        public virtual TEntity Find(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        /// <summary>
        /// This method use to get an entity by params entity id input
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync(params object[] entityIds)
        {
            return await _dbSet.FindAsync(entityIds);
        }

        /// <summary>
        /// This method use to get an entity by params entity id input
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        /// <summary>
        /// This method use to find a list entity by using expression with special condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Finds(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        #endregion

        #region Adds

        //// Add Options

        /// <summary>
        /// This method use to add Added Track to an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// This method use to add async entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// This method use to add Added Track to an array entity
        /// </summary>
        /// <param name="entities"></param>
        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// This method use to add range async entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public virtual async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Updates

        //// Update Options

        /// <summary>
        /// This method use to add modified track to an entity
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// This method use to add modified track to multiple entity
        /// </summary>
        /// <param name="entity"></param>
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
