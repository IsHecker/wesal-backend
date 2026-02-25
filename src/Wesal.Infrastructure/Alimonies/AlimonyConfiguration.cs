using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Infrastructure.Alimonies;

internal sealed class AlimonyConfiguration : IEntityTypeConfiguration<Alimony>
{
    public void Configure(EntityTypeBuilder<Alimony> builder)
    {
        builder.HasOne<FamilyCourt>()
            .WithMany()
            .HasForeignKey(alimony => alimony.CourtId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<CourtCase>()
            .WithOne()
            .HasForeignKey<Alimony>(alimony => alimony.CourtCaseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(alimony => alimony.FamilyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(alimony => alimony.PayerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(alimony => alimony.RecipientId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}