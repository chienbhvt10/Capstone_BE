using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(Subject entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(Subject entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<Subject, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<Subject, bool> condition, bool isHardDeleted = false);
    }
}
