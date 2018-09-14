using Microsoft.EntityFrameworkCore;

namespace DataManagementSystem.Repositories
{
    // the EF context
    public class DataManagementSystemContext : DbContext
    {
        // entities
        public virtual DbSet<DbFileNode> DbFileNodes { get; set; }

        public DataManagementSystemContext(DbContextOptions<DataManagementSystemContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new DbFileNodeConfiguration());
        }
    }
}
