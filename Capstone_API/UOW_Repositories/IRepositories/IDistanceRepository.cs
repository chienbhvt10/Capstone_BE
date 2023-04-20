using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface IDistanceRepository : IGenericRepository<Distance>
    {
        IEnumerable<Distance> MappingDistanceData();
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(Distance entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(Distance entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<Distance, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<Distance, bool> condition, bool isHardDeleted = false);
    }
}
