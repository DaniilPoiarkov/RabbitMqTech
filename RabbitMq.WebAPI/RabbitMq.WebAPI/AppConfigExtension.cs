using Microsoft.EntityFrameworkCore;
using RabbitMq.DAL;

namespace RabbitMq.WebAPI
{
    public static class AppConfigExtension
    {
        public static void ApplyPendingMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<RabbitMqDb>();

            if (db.Database.GetPendingMigrations().Any())
                db.Database.Migrate();
        }
    }
}
