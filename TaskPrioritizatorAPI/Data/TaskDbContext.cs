using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class TaskDbContext : DbContext
    {
        public TaskDbContext()
        {
            
        }

        public TaskDbContext(DbContextOptions<TaskDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Model.Task> Tasks { get; set; }
    }
}
