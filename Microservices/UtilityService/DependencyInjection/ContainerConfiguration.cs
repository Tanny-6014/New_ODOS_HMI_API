using UtilityService.Interface;
using UtilityService.Repositories;

namespace UtilityService.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUtilities, UtilitiesDal>();
            services.AddTransient<ICopyWBSService, CopyWBSService>();
            services.AddTransient<ICopyGroupMarkingService, CopyGroupMarkingService>();
        }
    }
}
