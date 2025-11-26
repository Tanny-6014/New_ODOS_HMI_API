using ProductCodeMasterService.Interfaces;
using ProductCodeMasterService.Repositories;

namespace ProductCodeMasterService.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IProductCode, ProductCodeRepository>();

        }

    }
}