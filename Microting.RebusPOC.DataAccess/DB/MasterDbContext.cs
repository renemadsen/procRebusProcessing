using Microsoft.EntityFrameworkCore;
using Microting.RebusPOC.Infrastructure.POCO;

namespace Microting.RebusPOC.DataAccess
{
    public class MasterDbContext: DbContext
    {
        public MasterDbContext(DbContextOptions<MasterDbContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
