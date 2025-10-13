using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Interface;
using MyProject.Infrastructure.Data;
using MyProject.Infrastructure.Repositories;
using MyProject.Infrastructure.Service;
using StackExchange.Redis;

namespace MyProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDi(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Register application services here
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            var redisConnectionString = configuration.GetConnectionString("RedisConnection");
            services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var configuration = ConfigurationOptions.Parse(redisConnectionString);
                configuration.AbortOnConnectFail = false;
                configuration.ConnectTimeout = 5000;
                configuration.SyncTimeout = 5000;
                return ConnectionMultiplexer.Connect(configuration);
            });


            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRedisService, RedisService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IOtpService, OtpService>();

            return services;
        }
    }
}
