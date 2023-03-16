using System.Linq.Expressions;

namespace Capstone_API.UOW_Repositories.Infrastructures
{
    public interface IGenericRepository<TEntity>
    where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        TEntity GetById(params object[] keyValues);
        Task<TEntity> GetByIdAsync(params object[] keyValues);
        IEnumerable<TEntity> GetByCondition(Func<TEntity, bool> condition);
        TEntity Find(object entityId);
        TEntity Find(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> FindAsync(params object[] entityIds);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> expression);
        IEnumerable<TEntity> Finds(Expression<Func<TEntity, bool>> expression);
        void Add(TEntity entity);
        Task AddAsync(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void UpdateRange(IEnumerable<TEntity> entities);
    }
}
