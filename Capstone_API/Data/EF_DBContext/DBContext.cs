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
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(s_loggerFactory);
            optionsBuilder.UseSqlServer(
                "Data Source=DESKTOP-023CA5I;" +
                "Initial Catalog=EF_DB_Learn;" +
                "Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ClassConfiguration());
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