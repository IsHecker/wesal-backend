using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Infrastructure.Notifications;

internal sealed class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(notif => notif.RecipientId);
    }
}