
using Microsoft.EntityFrameworkCore;
using RabbitMq.DAL.EntityConfiguration;

namespace RabbitMq.DAL
{
    internal static class EntityConfigurationExtension
    {
        public static void ApplyModelConfigurations(this ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        }
    }
}
