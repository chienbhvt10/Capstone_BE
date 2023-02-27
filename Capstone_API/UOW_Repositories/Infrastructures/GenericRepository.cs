using Capstone_API.Enum;
using Capstone_API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Capstone_API.UOW_Repositories.Infrastructures
{
    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
       where TEntity : class, IBaseEntity
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
        /// This method use to get an entity by params entity id input
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindAsync(params object[] entityIds)
        {
            return await _dbSet.FindAsync(entityIds);
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

        #region Counts

        /// <summary>
        /// This method use to count entity
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <returns></returns>
        public virtual int Count(bool isHardDeleted = false)
        {
            return isHardDeleted == false ? _dbSet.Count(x => x.ExistStatus != Status.Deleted) : _dbSet.Count();
        }

        /// <summary>
        /// This method use to count entity with async
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <returns></returns>
        public virtual async Task<int> CountAsync(bool isHardDeleted = false)
        {
            if (isHardDeleted == false)
                return await _dbSet.CountAsync(x => x.ExistStatus != Status.Deleted);

            return await _dbSet.CountAsync();
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

        #endregion

        #region Deletes

        //// Delete Options

        /// <summary>
        /// This method use to delete and entity by id
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="isHardDeleted"></param>
        public virtual void Delete(int entityId, bool isHardDeleted = false)
        {
            var entity = _dbSet.FirstOrDefault(x => x.Id.Equals(entityId));

            if (entity == null)
                throw new ArgumentNullException($"{entityId} was not found in the {typeof(TEntity)}");

            if (isHardDeleted == false)
            {
                entity.ExistStatus = Status.Deleted;
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _dbSet.Remove(entity);
        }

        /// <summary>
        /// This method use to delete an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isHardDeleted"></param>
        public virtual void Delete(TEntity entity, bool isHardDeleted = false)
        {
            var entityExist = _dbSet.FirstOrDefault(x => x.Id.Equals(entity.Id));

            if (entityExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TEntity)}");

            if (isHardDeleted == false)
            {
                entity.ExistStatus = Status.Deleted;
                Context.Entry(entity).State = EntityState.Modified;
                return;
            }

            _dbSet.Remove(entity);
        }

        /// <summary>
        /// This method use to delete an array entities
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="keyValues"></param>
        public virtual void Delete(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = _dbSet.Find(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException($"{string.Join(";", keyValues)} was not found in the {typeof(TEntity)}");

            if (isHardDeleted == false)
            {

                entitiesExist.ExistStatus = Status.Deleted;
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }

            _dbSet.Remove(entitiesExist);
        }

        /// <summary>
        /// This method use to delete an array by entity id
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="entity"></param>
        public virtual async Task DeleteAsync(TEntity entity, bool isHardDeleted = false)
        {
            var entitiesExist = await _dbSet.FirstOrDefaultAsync(x => x.Id.Equals(entity.Id));

            if (entitiesExist == null)
                throw new ArgumentNullException($"{entity.Id} was not found in the {typeof(TEntity)}");

            if (isHardDeleted == false)
            {
                entitiesExist.ExistStatus = Status.Deleted;
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _dbSet.Remove(entitiesExist);
        }

        /// <summary>
        /// This method use to delete a params objects key value
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues)
        {
            var entitiesExist = await _dbSet.FindAsync(keyValues);

            if (entitiesExist == null)
                throw new ArgumentNullException(
                    $"{string.Join(";", keyValues)} was not found in the {typeof(TEntity)}");

            if (isHardDeleted == false)
            {
                entitiesExist.ExistStatus = Status.Deleted;
                Context.Entry(entitiesExist).State = EntityState.Modified;
                return;
            }
            _dbSet.Remove(entitiesExist);
        }

        /// <summary>
        /// This method use to delete entity by some condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isHardDeleted"></param>
        public virtual void DeleteByCondition(Func<TEntity, bool> condition, bool isHardDeleted = false)
        {
            var query = _dbSet.Where(condition);
            foreach (var entity in query)
            {
                Delete(entity, isHardDeleted);
            }
        }

        /// <summary>
        /// This method use to delete entity by some condition by async
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isHardDeleted"></param>
        /// <returns></returns>
        public virtual async Task DeleteByConditionAsync(Func<TEntity, bool> condition, bool isHardDeleted = false)
        {
            var query = _dbSet.Where(condition);
            foreach (var entity in query)
            {
                await DeleteAsync(entity, isHardDeleted);
            }
        }

        #endregion

    }
}
