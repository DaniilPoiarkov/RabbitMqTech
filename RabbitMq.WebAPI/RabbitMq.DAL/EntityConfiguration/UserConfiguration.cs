using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RabbitMq.Common.Entities;

namespace RabbitMq.DAL.EntityConfiguration
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(u => u.Email).IsUnique();

            builder
                .HasMany(u => u.Notifications)
                .WithOne()
                .HasForeignKey(n => n.RecieverId);
        }
    }
}
