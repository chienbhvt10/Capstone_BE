using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ISubjectPreferenceLevelRepository : IGenericRepository<SubjectPreferenceLevel>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(SubjectPreferenceLevel entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(SubjectPreferenceLevel entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<SubjectPreferenceLevel, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<SubjectPreferenceLevel, bool> condition, bool isHardDeleted = false);
    }
}
