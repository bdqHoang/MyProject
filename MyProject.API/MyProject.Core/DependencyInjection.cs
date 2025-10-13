using Microsoft.Extensions.DependencyInjection;

namespace MyProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCoreDi(this IServiceCollection services)
        {
            // Register application services here
            return services;
        }
    }
}
