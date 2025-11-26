using OrderService.Interfaces;
using OrderService.Repositories;

namespace OrderServices.DependencyInjection
{
    public static class ContainerConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IOrder, OrderRepositories>();
            services.AddTransient<IESMOrderProcessingService, ESMOrderProcessingService>();
            services.AddTransient<IPrecastService, PrecastService>();
            services.AddTransient<IOrderAssignment, OrderAssignmentRepository>();
            services.AddTransient<IInterface, InterfaceRepository>();
        }

    }
}