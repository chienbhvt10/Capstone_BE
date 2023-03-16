using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ILecturerQuotaRepository : IGenericRepository<LecturerQuotum>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(LecturerQuotum entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(LecturerQuotum entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<LecturerQuotum, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<LecturerQuotum, bool> condition, bool isHardDeleted = false);
    }
}
