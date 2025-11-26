
using DrainService.Interfaces;
using DrainService.Repositories;

namespace DrainService.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAdminDal, AdminDal>();
            services.AddTransient<IDetailDal, DetailDal>();
            services.AddTransient<IBorePileParameterSetService, BorePileParameterSetService>();
            services.AddTransient<BOMAbs, BOMDal>();
            services.AddTransient<ICABDAL, CABDAL>();




        }
    }
}