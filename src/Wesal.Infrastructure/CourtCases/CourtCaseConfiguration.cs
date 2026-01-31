using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;

namespace Wesal.Infrastructure.CourtCases;

internal sealed class CourtCaseConfiguration : IEntityTypeConfiguration<CourtCase>
{
    public void Configure(EntityTypeBuilder<CourtCase> builder)
    {
        builder.HasOne<FamilyCourt>()
            .WithMany()
            .HasForeignKey(courtCase => courtCase.CourtId);

        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(courtCase => courtCase.FamilyId);

        builder.HasOne<Document>()
            .WithMany()
            .HasForeignKey(courtCase => courtCase.DocumentId);
    }
}