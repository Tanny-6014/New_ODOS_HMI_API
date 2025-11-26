using CommonServices.Interfaces;
using CommonServices.Repositories;

namespace CommonServices.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICommonService, CommonServiceRepositories>();
        }

    }
}