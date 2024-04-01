using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using TaskManager.Database.Models; 

namespace TaskManager.Database; 

public class TaskManagerContext : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    
    public DbSet<FileModel> FileModels { get; set; }

    public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
        : base(options)
    {
        this.ChangeTracker.LazyLoadingEnabled = true;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<UserModel>()
            .HasIndex(u => u.Email)
            .IsUnique();
        builder.Entity<UserModel>()
            .HasIndex(u => u.FullName)
            .IsUnique();

    }
} 