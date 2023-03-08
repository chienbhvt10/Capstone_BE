using Capstone_API.Models;
using Capstone_API.UOW_Repositories.Infrastructures;
using Microsoft.EntityFrameworkCore;

namespace Capstone_API.UOW_Repositories.IRepositories
{
    public class TaskRepository : GenericRepository<TaskAssign>, ITaskRepository
    {
        CapstoneDataContext _context;
        public TaskRepository(CapstoneDataContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<TaskAssign> MappingTaskData()
        {
            var items = _context.TaskAssigns
                .Include(task => task.Class)
                .Include(task => task.Subject)
                .Include(task => task.Lecturer)
                .Include(task => task.TimeSlot).Select(item => item);
            return items;
        }
    }
}
