using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Schools;
using Wesal.Domain.Entities.Users;

namespace Wesal.Infrastructure.Schools;

internal sealed class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<School>(school => school.UserId);

        builder.HasIndex(school => school.Email).IsUnique();
    }
}