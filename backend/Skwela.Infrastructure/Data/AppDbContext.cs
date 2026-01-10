using Microsoft.EntityFrameworkCore;
using Skwela.Domain.Entities;
using Skwela.Domain.Enums;

namespace Skwela.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    //public DbSet<Classroom> Classrooms => Set<Classroom>();
    //public DbSet<Assignment> Assignments => Set<Assignment>();
    //public DbSet<Submission> Submissions => Set<Submission>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.Role).IsRequired().HasDefaultValue(UserRole.Student);
        });
    }
}