using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Capstone_API.Models
{
    public partial class CapstoneDataContext : DbContext
    {
        public CapstoneDataContext()
        {
        }

        public CapstoneDataContext(DbContextOptions<CapstoneDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<Distance> Distances { get; set; } = null!;
        public virtual DbSet<Lecturer> Lecturers { get; set; } = null!;
        public virtual DbSet<LecturerRegister> LecturerRegisters { get; set; } = null!;
        public virtual DbSet<Model> Models { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<SlotDay> SlotDays { get; set; } = null!;
        public virtual DbSet<SlotPreferenceLevel> SlotPreferenceLevels { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<SubjectPreferenceLevel> SubjectPreferenceLevels { get; set; } = null!;
        public virtual DbSet<TaskAssign> TaskAssigns { get; set; } = null!;
        public virtual DbSet<TimeSlot> TimeSlots { get; set; } = null!;
        public virtual DbSet<TimeSlotCompatibility> TimeSlotCompatibilities { get; set; } = null!;
        public virtual DbSet<TimeSlotConflict> TimeSlotConflicts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=CapstoneData;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Building");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ShortName).HasMaxLength(50);
            });

            modelBuilder.Entity<Class>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Distance>(entity =>
            {
                entity.ToTable("Distance");

                entity.HasIndex(e => e.Building1Id, "IX_Distance_Building1Id");

                entity.HasIndex(e => e.Building2Id, "IX_Distance_Building2Id");

                entity.HasOne(d => d.Building1)
                    .WithMany(p => p.DistanceBuilding1s)
                    .HasForeignKey(d => d.Building1Id);

                entity.HasOne(d => d.Building2)
                    .WithMany(p => p.DistanceBuilding2s)
                    .HasForeignKey(d => d.Building2Id);
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.ToTable("Lecturer");

                entity.Property(e => e.CreateOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ShortName).HasMaxLength(50);

                entity.Property(e => e.UpdateOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<LecturerRegister>(entity =>
            {
                entity.ToTable("LecturerRegister");

                entity.HasIndex(e => e.LecturerId, "IX_LecturerRegister_LecturerId");

                entity.HasIndex(e => e.SubjectId, "IX_LecturerRegister_SubjectId");

                entity.HasIndex(e => e.TimeSlotId, "IX_LecturerRegister_TimeSlotId");

                entity.Property(e => e.Note).HasMaxLength(50);

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.LecturerRegisters)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.LecturerRegisters)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.LecturerRegisters)
                    .HasForeignKey(d => d.TimeSlotId);
            });

            modelBuilder.Entity<Model>(entity =>
            {
                entity.ToTable("Model");

                entity.Property(e => e.InputType).HasMaxLength(50);

                entity.Property(e => e.Solver).HasMaxLength(50);

                entity.Property(e => e.Strategy).HasMaxLength(50);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SlotDay>(entity =>
            {
                entity.ToTable("SlotDay");
            });

            modelBuilder.Entity<SlotPreferenceLevel>(entity =>
            {
                entity.ToTable("SlotPreferenceLevel");

                entity.HasIndex(e => e.LecturerId, "IX_SlotPreferenceLevel_LecturerId");

                entity.HasIndex(e => e.SemesterId, "IX_SlotPreferenceLevel_SemesterId");

                entity.HasIndex(e => e.TimeSlotInstanceId, "IX_SlotPreferenceLevel_TimeSlotInstanceId");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.SlotPreferenceLevels)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.SlotPreferenceLevels)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.TimeSlotInstance)
                    .WithMany(p => p.SlotPreferenceLevels)
                    .HasForeignKey(d => d.TimeSlotInstanceId);
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.Property(e => e.Code).HasMaxLength(50);

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_Subjects_Semester");
            });

            modelBuilder.Entity<SubjectPreferenceLevel>(entity =>
            {
                entity.ToTable("SubjectPreferenceLevel");

                entity.HasIndex(e => e.LecturerId, "IX_SubjectPreferenceLevel_LecturerId");

                entity.HasIndex(e => e.SemesterId, "IX_SubjectPreferenceLevel_SemesterId");

                entity.HasIndex(e => e.SubjectId, "IX_SubjectPreferenceLevel_SubjectId");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.SubjectPreferenceLevels)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.SubjectPreferenceLevels)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.SubjectPreferenceLevels)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TaskAssign>(entity =>
            {
                entity.ToTable("TaskAssign");

                entity.Property(e => e.CreateOn).HasColumnType("datetime");

                entity.Property(e => e.Department).HasMaxLength(50);

                entity.Property(e => e.Slot1).HasMaxLength(50);

                entity.Property(e => e.Slot2).HasMaxLength(50);

                entity.Property(e => e.UpdateOn).HasColumnType("datetime");

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TaskAssigns)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_TaskAssign_Classes");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.TaskAssigns)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_TaskAssign_Semester");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.TaskAssigns)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK_TaskAssign_Subjects");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.TaskAssigns)
                    .HasForeignKey(d => d.TimeSlotId)
                    .HasConstraintName("FK_TaskAssign_TimeSlot");
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.ToTable("TimeSlot");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TimeSlotCompatibility>(entity =>
            {
                entity.ToTable("TimeSlotCompatibility");

                entity.HasIndex(e => e.TimeSlotId, "IX_TimeSlotCompatibility_TimeSlotId");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.TimeSlotCompatibilities)
                    .HasForeignKey(d => d.TimeSlotId);
            });

            modelBuilder.Entity<TimeSlotConflict>(entity =>
            {
                entity.ToTable("TimeSLotConflict");

                entity.HasIndex(e => e.TimeSlotId, "IX_TimeSLotConflict_TimeSlotId");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.TimeSlotConflicts)
                    .HasForeignKey(d => d.TimeSlotId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
