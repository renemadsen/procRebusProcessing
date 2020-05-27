using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microting.RebusPOC.DataAccess;
using Microting.RebusPOC.Infrastructure;
using Microting.RebusPOC.Infrastructure.DB;
using Microting.RebusPOC.Infrastructure.Messages;
using Microting.RebusPOC.Infrastructure.Rebus;
using Microting.RebusPOC.Service;
using Rebus.Config;
using Rebus.Persistence.FileSystem;
using Rebus.Routing.TypeBased;
using Rebus.ServiceProvider;

namespace Microting.RebusPOC
{
    public class Startup
    {
 
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            RebusConfig rebusConfig = Configuration.GetValue<RebusConfig>("RabbitMq");

            services.AddControllers();

            services.AddMainServices();
            services.AddRebus(configure =>
                configure.Logging(l => l.Console())
                    .Transport(t => t.UseRabbitMqAsOneWayClient($"amqp://{rebusConfig.Username}:{rebusConfig.Password}@{rebusConfig.Host}"))
                    .Options(o =>
                    {
                        o.SetMaxParallelism(1);
                        o.SetNumberOfWorkers(1);
                    }));

            services.AddDbContext<MasterDbContext>(options =>
                        options.UseMySQL(Configuration.GetConnectionString("MasterDB")));
            services.AddDbContext<SlaveDbContext>(options =>
                        options.UseMySQL(Configuration.GetConnectionString("Slave1DB")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplicationServices.UseRebus();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
