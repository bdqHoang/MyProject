using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyProject.Application.Behaviors;
using MyProject.Core.Entities;

namespace MyProject.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDi(this IServiceCollection services)
        {
            // Register application services here
            services.AddScoped<IPasswordHasher<Users>, PasswordHasher<Users>>();
            services.AddAutoMapper(cfg => {
                cfg.LicenseKey = "eyJhbGciOiJSUzI1NiIsImtpZCI6Ikx1Y2t5UGVubnlTb2Z0d2FyZUxpY2Vuc2VLZXkvYmJiMTNhY2I1OTkwNGQ4OWI0Y2IxYzg1ZjA4OGNjZjkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2x1Y2t5cGVubnlzb2Z0d2FyZS5jb20iLCJhdWQiOiJMdWNreVBlbm55U29mdHdhcmUiLCJleHAiOiIxNzkwODk5MjAwIiwiaWF0IjoiMTc1OTM3MzUyNiIsImFjY291bnRfaWQiOiIwMTk5YTJkNGM4YTQ3ZDdjOWNjN2MwOWI4NjUyNDdmOCIsImN1c3RvbWVyX2lkIjoiY3RtXzAxazZoZGIxMmRyeTlubmE5OTI2NTZ2cThuIiwic3ViX2lkIjoiLSIsImVkaXRpb24iOiIwIiwidHlwZSI6IjIifQ.JfrGfC9EvGczrj3JWq6xYAy5B1f5_hKz7pgHVqxPjh1aG_LwbRV_-nc2J9a6bt5M7alN7Kh_HyYjkR8-WLdacGy09BfYo-SLyKCmn1qE_5Pe-CwCttuo111tagS_oWeKZMT4MuHpUHAXbZ_-48-KumW7i9TIdpUr3aAstH9wAtiy3sUWlb1X64BU5n0Az1vEs2MjrKSRTiukoKJXvT8Cs75cDKXsctk1P1Q_oWzioJT1eT-d0GLVW1FBWLdEe3QOVybMCLJuqxo2oWv74kMj4zXeRdeKVCOH7ouh3YEUvAREXySQt_WkdH3HXYsxm5eyhkh9I2hiIY4q5v3ZTmPGYw";
            },typeof(DependencyInjection).Assembly);

            // MediatR
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
            
            return services;
        }
    }
}
