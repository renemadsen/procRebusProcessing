using Microsoft.Extensions.DependencyInjection;
using Microting.RebusPOC.Service.Managers;

namespace Microting.RebusPOC.Service
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddMainServices(this IServiceCollection services)
        {
            services.AddScoped<IMasterManager, MasterManager>();

            return services;
        }
    }
}
