using System.Linq.Expressions;

namespace Capstone_API.UOW_Repositories.Infrastructures
{
    public interface IGenericRepository<TEntity>
    where TEntity : class, IBaseEntity
    {
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        TEntity GetById(params object[] keyValues);
        Task<TEntity> GetByIdAsync(params object[] keyValues);
        IEnumerable<TEntity> GetByCondition(Func<TEntity, bool> condition);

        /// <summary>
        /// This method use to find by entity id 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>return an Entity exist</returns>
        TEntity Find(object entityId);

        /// <summary>
        /// This method use to get an entity by params entity id input
        /// </summary>
        /// <param name="entityIds"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(params object[] entityIds);

        /// <summary>
        /// This method use to find a list entity by using expression with special condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Finds(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// This method use to add Added Track to an entity
        /// </summary>
        /// <param name="entity"></param>
        void Add(TEntity entity);

        /// <summary>
        /// This method use to add async entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// This method use to add Added Track to an array entity
        /// </summary>
        /// <param name="entities"></param>
        void AddRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// This method use to add range async entities
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// This method use to count entity
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <returns></returns>
        int Count(bool isHardDeleted = false);

        /// <summary>
        /// This method use to count entity with async
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <returns></returns>
        Task<int> CountAsync(bool isHardDeleted = false);

        /// <summary>
        /// This method use to add modified track to an entity
        /// </summary>
        /// <param name="entity"></param>
        void Update(TEntity entity);

        /// <summary>
        /// This method use to delete and entity by id
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="isHardDeleted"></param>
        void Delete(int entityId, bool isHardDeleted = false);

        /// <summary>
        /// This method use to delete an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isHardDeleted"></param>
        void Delete(TEntity entity, bool isHardDeleted = false);

        /// <summary>
        /// This method use to delete an array entities
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="keyValues"></param>
        void Delete(bool isHardDeleted = false, params object[] keyValues);

        /// <summary>
        /// This method use to delete an array by entity id
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="entity"></param>
        Task DeleteAsync(TEntity entity, bool isHardDeleted = false);

        /// <summary>
        /// This method use to delete a params objects key value
        /// </summary>
        /// <param name="isHardDeleted"></param>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);

        /// <summary>
        /// This method use to delete entity by some condition
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isHardDeleted"></param>
        void DeleteByCondition(Func<TEntity, bool> condition, bool isHardDeleted = false);

        /// <summary>
        /// This method use to delete entity by some condition by async
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="isHardDeleted"></param>
        /// <returns></returns>
        Task DeleteByConditionAsync(Func<TEntity, bool> condition, bool isHardDeleted = false);
    }
}
