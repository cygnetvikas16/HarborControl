using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HarborControl.Models
{
    public partial class HarborControlContext : DbContext
    {
        public HarborControlContext()
        {
        }

        public HarborControlContext(DbContextOptions<HarborControlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BoatMaster> BoatMaster { get; set; }
        public virtual DbSet<BoatSchedule> BoatSchedule { get; set; }
        public virtual DbSet<BoatStatus> BoatStatus { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //        optionsBuilder.UseSqlServer("Server=CIPL-PC85;Database=HarborControl;Integrated Security=True");
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoatMaster>(entity =>
            {
                entity.Property(e => e.BoatType).HasMaxLength(50);

                entity.Property(e => e.Speed).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<BoatSchedule>(entity =>
            {
                entity.Property(e => e.BoatName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifyDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessEndDate).HasColumnType("datetime");

                entity.Property(e => e.ProcessStartDate).HasColumnType("datetime");

                entity.HasOne(d => d.BoatMaster)
                    .WithMany(p => p.BoatSchedule)
                    .HasForeignKey(d => d.BoatMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoatSchedule_BoatMaster");

                entity.HasOne(d => d.BoatStatusNavigation)
                    .WithMany(p => p.BoatSchedule)
                    .HasForeignKey(d => d.BoatStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BoatSchedule_BoatStatus");
            });

            modelBuilder.Entity<BoatStatus>(entity =>
            {
                entity.Property(e => e.Status).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
