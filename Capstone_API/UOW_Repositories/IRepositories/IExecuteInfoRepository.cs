﻿using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public interface IExecuteInfoRepository : IGenericRepository<ExecuteInfo>
    {
        public void Delete(int entityId, bool isHardDeleted = false);
        public void Delete(ExecuteInfo entity, bool isHardDeleted = false);
        public void Delete(bool isHardDeleted = false, params object[] keyValues);
        public Task DeleteAsync(ExecuteInfo entity, bool isHardDeleted = false);
        public Task DeleteAsync(bool isHardDeleted = false, params object[] keyValues);
        public void DeleteByCondition(Func<ExecuteInfo, bool> condition, bool isHardDeleted = false);
        public Task DeleteByConditionAsync(Func<ExecuteInfo, bool> condition, bool isHardDeleted = false);
    }
}
