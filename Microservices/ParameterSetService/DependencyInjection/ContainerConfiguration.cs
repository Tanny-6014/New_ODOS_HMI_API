using ParameterSetService.Interfaces;
using ParameterSetService.Repositories;

namespace ParameterSetService.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {

            services.AddTransient<IParameterSet,ParameterSetRepository>();
        }

    }
}