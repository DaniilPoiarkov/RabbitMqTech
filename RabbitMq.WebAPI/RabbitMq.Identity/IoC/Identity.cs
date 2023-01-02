using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RabbitMq.Identity.Abstract;
using RabbitMq.Identity.AuthServices;
using RabbitMq.Identity.Options;
using System.Net;
using System.Text;

namespace RabbitMq.Identity.IoC
{
    public static class Identity
    {
        public static IServiceCollection ApplyJwtConfiguration(this IServiceCollection services, JwtOptions options)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = options.Issuer,

                        ValidateAudience = true,
                        ValidAudience = options.Audience,

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key)),
                    };

                    opt.Events = new()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                context.Response.Headers.Add("token-expired", "true");

                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return Task.CompletedTask;
                        }
                    };
                });

            services
                .AddSingleton(options)
                .AddSingleton<JwtTokenFactory>()

                .AddTransient<IAuthService, AuthService>();

            return services;
        }
    }
}
