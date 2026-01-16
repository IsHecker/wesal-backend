using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Schools;

namespace Wesal.Infrastructure.Children;

internal sealed class ChildConfiguration : IEntityTypeConfiguration<Child>
{
    public void Configure(EntityTypeBuilder<Child> builder)
    {
        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(child => child.FamilyId);

        builder.HasOne<School>()
            .WithMany()
            .HasForeignKey(child => child.SchoolId);
    }
}