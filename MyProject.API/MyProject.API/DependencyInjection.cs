using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyProject.API.Middlewares;
using MyProject.Application;
using MyProject.Application.Common.Models;
using MyProject.Infrastructure;
using System.Text;

namespace MyProject.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiDi(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettingsSection = configuration.GetSection("JwtSettings");
            var jwtSettings = jwtSettingsSection.Get<JwtSettings>();  // Bind full object, an toàn hơn

            // Chain các extensions trả về IServiceCollection
            services.AddApplicationDi()
                    .AddInfrastructureDi(configuration)
                    .AddExceptionHandler<GlobalExceptionHandler>()
                    .AddProblemDetails()
                    .Configure<JwtSettings>(jwtSettingsSection);  // Bind cho IOptions

            // Gọi riêng các methods void hoặc special return
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>  // Có thể đổi tên thành "CorsPolicy" và restrict
                    policy.AllowAnyOrigin()  // Production: .WithOrigins("https://example.com")
                          .AllowAnyMethod()
                          .AllowAnyHeader());
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyProject API",
                    Version = "v1",
                    Description = "API documentation for MyProject"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Nhập token vào đây (chỉ cần dán token, không cần 'Bearer ')"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()  // OK với using System;
                    }
                });
            });

            // JWT Auth
            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
            {
                throw new InvalidOperationException("JWT Settings invalid.");  // Early fail
            }

            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}