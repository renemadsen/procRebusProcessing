using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microting.RebusPOC.Infrastructure.DB
{
    public class Record
    {
        public long Id { get; set; }

        public string WorkerId { get; set; }

        [Key]
        public string WorkId { get; set; }
    }
}