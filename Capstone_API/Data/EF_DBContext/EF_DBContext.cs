using Capstone_API.Data.Config;
using Capstone_API.Data.Entities;
using Exercise.Data.Config;
using Exercise.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Exercise.Data.EF_DBContext
{
    public class ExerciseDBContext : DbContext
    {
        private static readonly ILoggerFactory s_loggerFactory = LoggerFactory.Create(
            (builder) =>
            {
                builder.AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information);
                //builder.AddFilter(DbLoggerCategory.Database.Name, LogLevel.Information);
            }
            );

        private static ExerciseDBContext? s_instance;

        public ExerciseDBContext()
        {
        }

        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Class> Classes { get; set; }

        public static ExerciseDBContext Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new ExerciseDBContext();
                }
                return s_instance;
            }
            set => s_instance = value;
        }

        // Cấu hình liên quan đến cơ sở dữ liệu
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("CapstoneData"));
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-J3B9EFD;Initial Catalog=CapstoneData;Integrated Security=True");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new BuildingConfiguration());
            modelBuilder.ApplyConfiguration(new ClassConfiguration());
            modelBuilder.ApplyConfiguration(new DistanceConfiguration());
            modelBuilder.ApplyConfiguration(new LecturerConfiguration());
            modelBuilder.ApplyConfiguration(new LecturerRegisterConfiguration());
            modelBuilder.ApplyConfiguration(new ModelConfiguration());
            modelBuilder.ApplyConfiguration(new SemesterConfiguration());
            modelBuilder.ApplyConfiguration(new SlotDayConfiguration());
            modelBuilder.ApplyConfiguration(new SlotPreferenceLevelConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectConfiguration());
            modelBuilder.ApplyConfiguration(new SubjectPreferenceLevelConfiguration());
            modelBuilder.ApplyConfiguration(new TaskAssignConfiguration());
            modelBuilder.ApplyConfiguration(new TimeSlotCompatibilityConfiguration());
            modelBuilder.ApplyConfiguration(new TimeSLotConfiguration());
            modelBuilder.ApplyConfiguration(new TimeSlotConflictConfiguration());
        }

        public static void CreateDataBase()
        {
            try
            {
                using var dbcontext = Instance;
                dbcontext.Database.EnsureCreated();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void DropDataBase()
        {
            try
            {
                using var dbcontext = Instance;
                var kq = dbcontext.Database.EnsureDeleted();
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }

        }
    }
}