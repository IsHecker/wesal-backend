using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Parents;
using Wesal.Domain.Entities.UserDevices;

namespace Wesal.Infrastructure.UserDevices;

internal sealed class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(device => device.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(d => d.UserId);
        builder.HasIndex(d => d.DeviceToken);
    }
}