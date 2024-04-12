using Microsoft.EntityFrameworkCore;
using TaskManager.Controllers;
using TaskManager.Database.Models; // Убедитесь, что пространство имен Models доступно

namespace TaskManager.Database
{
    public class TaskManagerContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; } 
        public DbSet<FileModel> FileModels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Product> Products { get; set; } 

        public TaskManagerContext(DbContextOptions options)
            : base(options)
        {
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
}