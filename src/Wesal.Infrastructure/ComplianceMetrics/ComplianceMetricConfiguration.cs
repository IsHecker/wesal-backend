using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Compliances;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Infrastructure.ComplianceMetrics;

internal sealed class ComplianceMetricConfiguration : IEntityTypeConfiguration<ComplianceMetric>
{
    public void Configure(EntityTypeBuilder<ComplianceMetric> builder)
    {
        builder.HasOne<FamilyCourt>()
            .WithMany()
            .HasForeignKey(metric => metric.CourtId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(metric => metric.FamilyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(metric => metric.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(metric => new { metric.Date, metric.FamilyId, metric.ParentId }).IsUnique();
    }
}