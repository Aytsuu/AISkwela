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
            entity.HasKey(e => e.id);
            entity.Property(e => e.username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.password).IsRequired();
            entity.Property(e => e.role).IsRequired().HasDefaultValue(UserRole.Student);
            entity.Property(e => e.refreshToken);
            entity.Property(e => e.refreshTokenExpiryTime);
        });
    }
}