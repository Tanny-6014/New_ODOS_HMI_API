using ShapeCodeService.Interfaces;
using ShapeCodeService.Repositories;

namespace ShapeCodeService.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IShapeGroup, ShapeGroupService>();
            services.AddTransient<IShapeSurchage, ShapeSurchageRepository>();

            services.AddTransient<ICABDAL, CABDAL>();
            services.AddTransient<IAdminDal, AdminDal>();

        }

    }
}