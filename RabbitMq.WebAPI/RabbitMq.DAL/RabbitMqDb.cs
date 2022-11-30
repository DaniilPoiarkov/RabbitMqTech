using Microsoft.EntityFrameworkCore;
using RabbitMq.Common.Entities;
using RabbitMq.Common.Entities.Notifications;

namespace RabbitMq.DAL
{
    public class RabbitMqDb : DbContext
    {
        public RabbitMqDb(DbContextOptions<RabbitMqDb> options) : 
            base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder) => 
            modelBuilder.ApplyModelConfigurations();

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<SimpleNotification> SimpleNotifications { get; set; } = null!;
        public DbSet<PrivateNotification> PrivateNotifications { get; set; }
        public DbSet<PublicNotification> PublicNotifications { get; set; }
    }
}
