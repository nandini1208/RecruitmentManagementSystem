using Microsoft.EntityFrameworkCore;
using RecruitmentManagementSystem.Models;

namespace RecruitmentManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Simple configuration - NO cascade deletes
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<Profile>(p => p.UserId);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.PostedBy)
                .WithMany(u => u.PostedJobs)
                .HasForeignKey(j => j.PostedById);

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Applicant)
                .WithMany(u => u.Applications)
                .HasForeignKey(ja => ja.ApplicantId);

            modelBuilder.Entity<JobApplication>()
                .HasOne(ja => ja.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(ja => ja.JobId);

            // Ensure unique email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}