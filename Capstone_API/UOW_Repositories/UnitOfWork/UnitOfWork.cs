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
        IAreaSlotWeightRepository _areaSlotWeightRepository;
        IBuildingRepository _buildingRepository;
        IDistanceRepository _distanceRepository;
        ISlotPreferenceLevelRepository _slotPreferenceLevelRepository;
        ISubjectPreferenceLevelRepository _subjectPreferenceLevelRepository;
        ITimeSlotConflictRepository _timeSlotConflictRepository;
        ITimeSlotRepository _timeSlotRepository;
        IRoomRepository _roomRepository;
        IClassRepository _classRepository;
        IExecuteInfoRepository _executeInfoRepository;
        ITimeSlotSegmentRepository _timeSlotSegmentRepository;
        INumSegmentsRepository _numSegmentsRepository;
        IDayOfWeeksRepository _dayOfWeeksRepository;
        ISemesterRepository _semesterRepository;

        public UnitOfWork(CapstoneDataContext context)
        {
            Context = context;
        }

        public CapstoneDataContext Context { get; }


        public ITaskRepository TaskRepository => _taskAssignRepository ??= new TaskRepository(Context);
        public ILecturerRepository LecturerRepository => _lecturerRepository ??= new LecturerRepository(Context);
        public ISubjectRepository SubjectRepository => _subjectRepository ??= new SubjectRepository(Context);
        public IAreaSlotWeightRepository AreaSlotWeightRepository => _areaSlotWeightRepository ??= new AreaSlotWeightRepository(Context);
        public IBuildingRepository BuildingRepository => _buildingRepository ??= new BuildingRepository(Context);
        public IDistanceRepository DistanceRepository => _distanceRepository ??= new DistanceRepository(Context);
        public ISlotPreferenceLevelRepository SlotPreferenceLevelRepository => _slotPreferenceLevelRepository ??= new SlotPreferenceLevelRepository(Context);
        public ISubjectPreferenceLevelRepository SubjectPreferenceLevelRepository => _subjectPreferenceLevelRepository ??= new SubjectPreferenceLevelRepository(Context);
        public ITimeSlotConflictRepository TimeSlotConflictRepository => _timeSlotConflictRepository ??= new TimeSlotConflictRepository(Context);
        public ITimeSlotRepository TimeSlotRepository => _timeSlotRepository ??= new TimeSlotRepository(Context);
        public IRoomRepository RoomRepository => _roomRepository ??= new RoomRepository(Context);
        public IClassRepository ClassRepository => _classRepository ??= new ClassRepository(Context);
        public IExecuteInfoRepository ExecuteInfoRepository => _executeInfoRepository ??= new ExecuteInfoRepository(Context);
        public ITimeSlotSegmentRepository TimeSlotSegmentRepository => _timeSlotSegmentRepository ??= new TimeSlotSegmentRepository(Context);
        public INumSegmentsRepository NumSegmentsRepository => _numSegmentsRepository ??= new NumSegmentsRepository(Context);
        public IDayOfWeeksRepository DayOfWeeksRepository => _dayOfWeeksRepository ??= new DayOfWeeksRepository(Context);
        public ISemesterRepository SemesterRepository => _semesterRepository ??= new SemesterRepository(Context);

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
