using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Repositories;

namespace Capstone_API.UOW_Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        CapstoneDataContext Context { get; }
        ITaskRepository TaskRepository { get; }
        void Complete();
        Task<int> CompleteAsync();
    }
}
