namespace MyProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiDi(this IServiceCollection services, IConfiguration configuration)
        {
            // Register application services here
            services.AddApplicationDi()
                .AddInfrastructureDi(configuration);

            return services;
        }
    }
}
