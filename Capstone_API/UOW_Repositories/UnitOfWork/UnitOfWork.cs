using Capstone_API.Models;
using Capstone_API.UOW_Repositories.IRepositories;
using Capstone_API.UOW_Repositories.Repositories;

namespace Capstone_API.UOW_Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        ITaskRepository _taskAssignRepository;
        ILecturerRepository _lecturerRepository;
        ISubjectRepository _subjectRepository;

        public UnitOfWork(CapstoneDataContext context)
        {
            Context = context;
        }

        public CapstoneDataContext Context { get; }


        public ITaskRepository TaskRepository => _taskAssignRepository ??= new TaskRepository(Context);
        public ILecturerRepository LecturerRepository => _lecturerRepository ??= new LecturerRepository(Context);
        public ISubjectRepository SubjectRepository => _subjectRepository ??= new SubjectRepository(Context);


        public void Dispose()
        {
            Context.Dispose();
        }
        public void Complete()
        {
            Context.SaveChanges();
        }

        public Task<int> CompleteAsync()
        {
            return Context.SaveChangesAsync();
        }
    }
}
