using Microsoft.EntityFrameworkCore;
using MyWebAppProject;

namespace TaskApi;

public class TaskDbContext : DbContext
{
    public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
    {
    }
    public TaskDbContext() : base()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=tasks.db");
        }
    }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<MyWebAppProject.Models.User> Users { get; set; }
}