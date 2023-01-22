using Microsoft.OpenApi.Models;

namespace RabbitMq.WebAPI.DependencyInjection.ServiceInstallers
{
    public class SwaggerServiceInstaller : IServiceInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.AddSecurityDefinition("Bearer", new()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Bearer [space] [token]"
                });

                opt.AddSecurityRequirement(new()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    }
                });

                opt.SwaggerDoc("v1", new()
                {
                    Title = "RabbitMq",
                    Version = "1.0.1",
                    Description = "Simple implementation of following technologies:\n" +
                        "RabbitMQ, MediatR, Automapper, xUnit, Quartz, SignalR.\n\t\r" +
                        "Database: Postgres.\n\t\r" +
                        "Custom Identity implementation."
                });
            });
        }
    }
}
