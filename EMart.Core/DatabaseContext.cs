using EMart.Core.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace EMart.Core
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions)
              : base(dbContextOptions)
        {
        }

        public virtual DbSet<Configurations> Configurations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            base.OnModelCreating(modelBuilder);
        }
    }
}
