using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface ILecturerRepository : IGenericRepository<Lecturer>
    {
        IEnumerable<Lecturer> MappingLecturerData();
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(Lecturer entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(Lecturer entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<Lecturer, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<Lecturer, bool> condition, bool isHardDeleted = false);
    }
}
