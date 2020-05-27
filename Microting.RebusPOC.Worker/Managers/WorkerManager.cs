using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microting.RebusPOC.DataAccess;
using Microting.RebusPOC.Infrastructure.DB;
using Microting.RebusPOC.Infrastructure.Messages;
using Rebus.Handlers;
using System;
using System.Threading.Tasks;

namespace Microting.RebusPOC.Worker.Managers
{
    public class WorkerManager : IHandleMessages<WorkTriggeredMessage>
    {
        private readonly SlaveDbContext _dbContext;
        private readonly MasterDbContext _masterDbContext;
        private readonly ILogger<WorkerManager> _log;

        public WorkerManager(SlaveDbContext dbContext, MasterDbContext masterDbContext, ILogger<WorkerManager> log)
        {
            _dbContext = dbContext;
            _masterDbContext = masterDbContext;
            _log = log;
        }

        public async Task Handle(WorkTriggeredMessage message)
        {
            Console.WriteLine($"\"{{token: {message.Token}, id: {message.Id}, workId: {message.WorkId}}}\"");

            var customer = await _masterDbContext.Customers?.FirstOrDefaultAsync(t => t.Token == message.Token);
            if (customer?.Db == null)
                return;

            var con = _dbContext.Database.GetDbConnection();
            if (con.ConnectionString != customer.Db)
            {
                await _dbContext.Database.CloseConnectionAsync();

                con.ConnectionString = customer.Db;

                await _dbContext.Database.OpenConnectionAsync();
            }

            await _dbContext.Records.AddAsync(new Record { WorkId = message.WorkId, WorkerId = Program.WorkerId.ToString() });

            await _dbContext.SaveChangesAsync();
        }
    }
}
