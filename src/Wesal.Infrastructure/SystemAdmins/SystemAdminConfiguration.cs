using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.SystemAdmins;
using Wesal.Domain.Entities.Users;

namespace Wesal.Infrastructure.SystemAdmins;

internal sealed class SystemAdminConfiguration : IEntityTypeConfiguration<SystemAdmin>
{
    public void Configure(EntityTypeBuilder<SystemAdmin> builder)
    {
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<SystemAdmin>(admin => admin.UserId);

        builder.HasIndex(admin => admin.Email)
            .IsUnique();
    }
}
