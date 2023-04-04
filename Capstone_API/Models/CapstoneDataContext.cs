﻿using System;
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

        public virtual DbSet<AreaSlotWeight> AreaSlotWeights { get; set; } = null!;
        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<Class> Classes { get; set; } = null!;
        public virtual DbSet<DayOfWeek> DayOfWeeks { get; set; } = null!;
        public virtual DbSet<Distance> Distances { get; set; } = null!;
        public virtual DbSet<ExecuteInfo> ExecuteInfos { get; set; } = null!;
        public virtual DbSet<Lecturer> Lecturers { get; set; } = null!;
        public virtual DbSet<LecturerQuotum> LecturerQuota { get; set; } = null!;
        public virtual DbSet<LecturerRegister> LecturerRegisters { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Semester> Semesters { get; set; } = null!;
        public virtual DbSet<SlotPerDay> SlotPerDays { get; set; } = null!;
        public virtual DbSet<SlotPreferenceLevel> SlotPreferenceLevels { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<SubjectPreferenceLevel> SubjectPreferenceLevels { get; set; } = null!;
        public virtual DbSet<TaskAssign> TaskAssigns { get; set; } = null!;
        public virtual DbSet<TimeSlot> TimeSlots { get; set; } = null!;
        public virtual DbSet<TimeSlotConflict> TimeSlotConflicts { get; set; } = null!;
        public virtual DbSet<TimeSlotSegment> TimeSlotSegments { get; set; } = null!;

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
            modelBuilder.Entity<AreaSlotWeight>(entity =>
            {
                entity.ToTable("AreaSlotWeight");

                entity.Property(e => e.AreaSlotWeight1).HasColumnName("AreaSlotWeight");

                entity.HasOne(d => d.AreaSlot)
                    .WithMany(p => p.AreaSlotWeightAreaSlots)
                    .HasForeignKey(d => d.AreaSlotId)
                    .HasConstraintName("FK_AreaSlotWeight_TimeSlot1");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.AreaSlotWeightSlots)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK_AreaSlotWeight_TimeSlot");
            });

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

            modelBuilder.Entity<DayOfWeek>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

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

            modelBuilder.Entity<ExecuteInfo>(entity =>
            {
                entity.ToTable("ExecuteInfo");

                entity.Property(e => e.ExecuteTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Lecturer>(entity =>
            {
                entity.ToTable("Lecturer");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.ShortName).HasMaxLength(50);
            });

            modelBuilder.Entity<LecturerQuotum>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.LecturerQuota)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK_LecturerQuota_Lecturer");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.LecturerQuota)
                    .HasForeignKey(d => d.SemesterId)
                    .HasConstraintName("FK_LecturerQuota_Semester");
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

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Semester>(entity =>
            {
                entity.ToTable("Semester");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<SlotPerDay>(entity =>
            {
                entity.ToTable("SlotPerDay");
            });

            modelBuilder.Entity<SlotPreferenceLevel>(entity =>
            {
                entity.ToTable("SlotPreferenceLevel");

                entity.HasIndex(e => e.LecturerId, "IX_SlotPreferenceLevel_LecturerId");

                entity.HasIndex(e => e.SemesterId, "IX_SlotPreferenceLevel_SemesterId");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.SlotPreferenceLevels)
                    .HasForeignKey(d => d.LecturerId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.SlotPreferenceLevels)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.SlotPreferenceLevels)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK_SlotPreferenceLevel_TimeSlot");
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

                entity.HasOne(d => d.Class)
                    .WithMany(p => p.TaskAssigns)
                    .HasForeignKey(d => d.ClassId)
                    .HasConstraintName("FK_TaskAssign_Classes");

                entity.HasOne(d => d.Lecturer)
                    .WithMany(p => p.TaskAssigns)
                    .HasForeignKey(d => d.LecturerId)
                    .HasConstraintName("FK_TaskAssign_Lecturer");

                entity.HasOne(d => d.Room1)
                    .WithMany(p => p.TaskAssigns)
                    .HasForeignKey(d => d.Room1Id)
                    .HasConstraintName("FK_TaskAssign_Room");

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

                entity.Property(e => e.AmorPm).HasColumnName("AMorPM");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TimeSlotConflict>(entity =>
            {
                entity.ToTable("TimeSLotConflict");

                entity.HasOne(d => d.ConflictSlot)
                    .WithMany(p => p.TimeSlotConflictConflictSlots)
                    .HasForeignKey(d => d.ConflictSlotId)
                    .HasConstraintName("FK_TimeSLotConflict_TimeSlot");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.TimeSlotConflictSlots)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK_TimeSLotConflict_TimeSlot1");
            });

            modelBuilder.Entity<TimeSlotSegment>(entity =>
            {
                entity.ToTable("TimeSlotSegment");

                entity.HasOne(d => d.DayOfWeekNavigation)
                    .WithMany(p => p.TimeSlotSegments)
                    .HasForeignKey(d => d.DayOfWeek)
                    .HasConstraintName("FK_TimeSlotSegment_DayOfWeek");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.TimeSlotSegments)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("FK_TimeSlotSegment_TimeSlot1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
