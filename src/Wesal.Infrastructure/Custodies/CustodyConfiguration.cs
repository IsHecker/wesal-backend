using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Users;

namespace Wesal.Infrastructure.Custodies;

internal sealed class CustodyConfiguration : IEntityTypeConfiguration<Custody>
{
    public void Configure(EntityTypeBuilder<Custody> builder)
    {
        builder.HasOne<CourtCase>()
            .WithOne()
            .HasForeignKey<Custody>(custody => custody.CourtCaseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(custody => custody.FamilyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(custody => custody.CustodianId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}