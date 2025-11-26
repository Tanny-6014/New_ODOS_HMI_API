using WBSService.Controllers;
using WBSService.Interfaces;
using WBSService.Repositories;

namespace WBSService.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IWbs, WbsRepository>();
            services.AddHttpClient<WbsController>();
        }

    }
}
