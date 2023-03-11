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
        ISettingModelRepository SettingModelRepository { get; }
        ISlotPreferenceLevelRepository SlotPreferenceLevelRepository { get; }
        ISubjectPreferenceLevelRepository SubjectPreferenceLevelRepository { get; }
        ITimeSlotCompatibilityRepository TimeSlotCompatibilityRepository { get; }
        ITimeSlotConflictRepository TimeSlotConflictRepository { get; }
        ITimeSlotRepository TimeSlotRepository { get; }
        IRoomRepository RoomRepository { get; }
        IClassRepository ClassRepository { get; }


        void Complete();
        Task<int> CompleteAsync();
    }
}
