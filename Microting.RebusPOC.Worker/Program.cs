using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microting.RebusPOC.DataAccess;
using Microting.RebusPOC.Infrastructure;
using Microting.RebusPOC.Infrastructure.DB;
using Microting.RebusPOC.Infrastructure.Messages;
using Microting.RebusPOC.Infrastructure.Rebus;
using Microting.RebusPOC.Worker.Managers;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;
using Rebus.Handlers;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Microting.RebusPOC.Worker
{
    class Program
    {
        public static Guid WorkerId = Guid.NewGuid();
        static void Main(string[] args)
        {
            Console.WriteLine("Starting worker: " + WorkerId);

            var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(AppContext.BaseDirectory))
            .AddJsonFile("appsettings.json", optional: true);

            var configuration = builder.Build();

            RebusConfig rebusConfig = configuration.GetValue<RebusConfig>("RabbitMq");

            var serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddRebus(configure =>
                configure.Logging(l => l.Console())//.Logging(l => l.Use(new MSLoggerFactoryAdapter(new LoggerFactory())))
                    .Transport(t => t.UseRabbitMq($"amqp://{rebusConfig.Username}:{rebusConfig.Password}@{rebusConfig.Host}", rebusConfig.QueueName))
                    .Routing(r => r.TypeBased().MapAssemblyOf<WorkTriggeredMessage>("test"))
                    .Options(o =>
                    {
                        o.SetMaxParallelism(1);
                        o.SetNumberOfWorkers(1);
                    }))
                .AddDbContext<MasterDbContext>(options =>
                                     options.UseMySQL(configuration.GetConnectionString("MasterDB")))
                .AddDbContext<SlaveDbContext>(options =>
                                     options.UseMySQL(configuration.GetConnectionString("Slave1Db")))
                .AddScoped<IConfiguration>(c => configuration)
                .AddScoped<IHandleMessages<WorkTriggeredMessage>, WorkerManager>()
                .BuildServiceProvider();

            serviceProvider.UseRebus(bus =>
            {
                bus.Subscribe<WorkTriggeredMessage>().Wait();
            });

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

        }
    }

    class Handler : IHandleMessages<WorkTriggeredMessage>
    {
        public async Task Handle(WorkTriggeredMessage message)
        {
            Console.WriteLine("Got string: {0}", message.WorkId);
        }
    }
}
