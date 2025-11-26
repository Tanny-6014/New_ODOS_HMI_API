using DetailingService.Interfaces;
using DetailingService.Repositories;

namespace DetailingService.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IMeshDetailing, MeshDetailingService>();
            services.AddTransient<ISlabService, SlabService>();
            services.AddTransient<ICarpetService, CarpetService>();
            services.AddTransient<IColumnService, ColumnService>();
            services.AddTransient<IBeamService, BeamService>();
            services.AddTransient<IAccessories, AccessoriesServicecs>();
            services.AddTransient<IPRCDetailingService, PRCDetailingService>();
            services.AddTransient<IBOM, BomService>();
            services.AddTransient<IGroupMarkDal, GroupMarkDAL>();
            services.AddTransient<ICABService, CABService>();
            services.AddTransient<IAccountService, AccountService>();

        }

    }
}