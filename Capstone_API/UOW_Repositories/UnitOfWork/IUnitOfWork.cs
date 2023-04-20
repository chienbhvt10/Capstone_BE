using Capstone_API.Models;
using Capstone_API.UOW_Repositories.IRepositories;

namespace Capstone_API.UOW_Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        CapstoneDataContext Context { get; }
        ITaskRepository TaskRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        ILecturerRepository LecturerRepository { get; }
        IAreaSlotWeightRepository AreaSlotWeightRepository { get; }
        IBuildingRepository BuildingRepository { get; }
        IDistanceRepository DistanceRepository { get; }
        ISlotPreferenceLevelRepository SlotPreferenceLevelRepository { get; }
        ISubjectPreferenceLevelRepository SubjectPreferenceLevelRepository { get; }
        ITimeSlotConflictRepository TimeSlotConflictRepository { get; }
        ITimeSlotRepository TimeSlotRepository { get; }
        IRoomRepository RoomRepository { get; }
        IClassRepository ClassRepository { get; }
        IExecuteInfoRepository ExecuteInfoRepository { get; }
        ITimeSlotSegmentRepository TimeSlotSegmentRepository { get; }
        INumSegmentsRepository NumSegmentsRepository { get; }
        IDayOfWeeksRepository DayOfWeeksRepository { get; }
        ISemesterInfoRepository SemesterInfoRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IUserRepository UserRepository { get; }

        void Complete();
        Task<int> CompleteAsync();
    }
}
