
namespace Microting.RebusPOC.Infrastructure.Messages
{
    public class WorkTriggeredMessage
    {
        public long Id { get; set; }
        public string Token {get; set;}
        public string WorkId { get; set; }
    }
}
