using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RabbitMq.Common.Entities.Notifications;

namespace RabbitMq.DAL.EntityConfiguration
{
    internal class PrivateNotificationConfiguration : IEntityTypeConfiguration<PrivateNotification>
    {
        public void Configure(EntityTypeBuilder<PrivateNotification> builder)
        {
            builder.HasOne(n => n.Sender)
                .WithMany()
                .HasForeignKey(n => n.SenderId);
        }
    }
}
