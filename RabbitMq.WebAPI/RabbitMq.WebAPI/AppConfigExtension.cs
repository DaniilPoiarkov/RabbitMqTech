using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RabbitMq.Broker.DIRegistration;
using RabbitMq.Broker.Models.Options;
using RabbitMq.Common.Parameters;
using RabbitMq.DAL;
using RabbitMq.Identity.IoC;
using RabbitMq.Identity.Options;
using RabbitMq.Services.Abstract;
using RabbitMq.Services.Implementations;
using RabbitMq.Services.MappingProfiles;

namespace RabbitMq.WebAPI
{
    public static class AppConfigExtension
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSignalR();
            services.AddLogging();

            services
                .ConfigureDatabase(configuration)
                .AddIdentity(configuration)
                .ConfigureRabbitMqBroker(configuration)
                .ConfigureSwagger()
                .RegisterAutoMapper();

            services
                .AddTransient<IUserService, UserService>()
                .AddTransient<IQueueService, QueueService>()

                .AddScoped(typeof(UserParameters));
        }

        private static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<NotificationProfile>();                
            });

            return services;
        }

        private static IServiceCollection ConfigureRabbitMqBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new NotificationHubOptions();
            configuration.GetSection(nameof(NotificationHubOptions)).Bind(options);

            services.AddRabbitMqBroker(opt =>
            {
                opt.SimpleNotificationsQueue = options.SimpleNotificationsQueue;
                opt.PrivateNotificationsQueue = options.PrivateNotificationsQueue;
                opt.ExchangeName = options.ExchangeName;
            });

            return services;
        }

        private static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connection = configuration.GetConnectionString(nameof(RabbitMqDb));

            services.AddDbContext<RabbitMqDb>(o => o.UseNpgsql(connection,
                b => b.MigrationsAssembly(typeof(RabbitMqDb).Assembly.FullName))
            .EnableDetailedErrors());

            return services;
        }

        public static void ApplyPendingMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RabbitMqDb>();

            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();
        }

        private static IServiceCollection ConfigureSwagger(this IServiceCollection services)
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
                    Version = "1",
                    Description = "Simple implementation of RabbitMQ"
                });
            });

            return services;
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            var options = new JwtOptions();
            configuration.GetSection(nameof(JwtOptions)).Bind(options);

            services.RegisterIdentity(opt =>
            {
                opt.Issuer = options.Issuer;
                opt.Audience = options.Audience;
                opt.Key = options.Key;
                opt.ValidFor = options.ValidFor;
            });

            return services;
        }
    }
}
