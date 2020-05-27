using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microting.RebusPOC.DataAccess;
using Microting.RebusPOC.Infrastructure.Messages;
using Rebus.Bus;
using System;
using System.Threading.Tasks;

namespace Microting.RebusPOC.Service.Managers
{
    public class MasterManager : IMasterManager
    {
        private readonly ILogger<MasterManager> _logger;
        private readonly MasterDbContext _masterDbContext;
        private readonly IBus _bus;

        public MasterManager(ILogger<MasterManager> logger, MasterDbContext masterDbContext, IBus bus)
        {
            _logger = logger;
            _masterDbContext = masterDbContext;

            _bus = bus;
        }

        public async Task Trigger(long id)
        {
            try
            {
                var customer = await _masterDbContext.Customers?
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (customer != null)
                {
                    await _bus.Publish( new WorkTriggeredMessage() { Id = id, Token = customer.Token, WorkId = Guid.NewGuid().ToString()});
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured");
            }
        }
    }
}
