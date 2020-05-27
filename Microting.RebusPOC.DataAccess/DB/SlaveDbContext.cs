using Microsoft.EntityFrameworkCore;

namespace Microting.RebusPOC.Infrastructure.DB
{
    public class SlaveDbContext : DbContext
    {
        public SlaveDbContext(DbContextOptions<SlaveDbContext> options)
            : base(options)
        {

        }

        public DbSet<Record> Records { get; set; }
    }
}
