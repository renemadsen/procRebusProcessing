using System.ComponentModel.DataAnnotations;

namespace Microting.RebusPOC.Infrastructure.POCO
{
    public class Customer
    {
        [Key]
        public long Id { get; set; }

        public string Token { get; set; }

        public string Db { get; set; }
    }
}
