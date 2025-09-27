using Microsoft.EntityFrameworkCore;
using defectTracker.Models;

//Fluent API в Entity Framework Core (EF Core) 

namespace defectTracker.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>() //обращение к сущности чтобы задать правила
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CreatedDefects)
                .WithOne(d => d.CreatedBy)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.Restrict); //запрет каскадного удаления

            modelBuilder.Entity<User>()
                .HasMany(u => u.AssignedDefects)
                .WithOne(d => d.AssignedTo)
                .HasForeignKey(d => d.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Defects)
                .WithOne(d => d.Project)
                .HasForeignKey(d => d.ProjectId);

            modelBuilder.Entity<Defect>()
                .HasMany(d => d.Comments)
                .WithOne(c => c.Defect)
                .HasForeignKey(c => c.DefectId);

            modelBuilder.Entity<Defect>()
                .HasMany(d => d.Attachments)
                .WithOne(a => a.Defect)
                .HasForeignKey(a => a.DefectId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId);

            // Ограничения
            modelBuilder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<User>().Property(u => u.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Role>().Property(r => r.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Project>().Property(p => p.Name).IsRequired().HasMaxLength(255);
            modelBuilder.Entity<Defect>().Property(d => d.Title).IsRequired().HasMaxLength(500);
            modelBuilder.Entity<Defect>().Property(d => d.Status).HasMaxLength(50);
            modelBuilder.Entity<Defect>().Property(d => d.Priority).HasMaxLength(50);
        }
    }
}