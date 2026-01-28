using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.CourtCases;
using Wesal.Domain.Entities.Custodies;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Parents;

namespace Wesal.Infrastructure.CustodyRequests;

internal sealed class CustodyRequestConfiguration : IEntityTypeConfiguration<CustodyRequest>
{
    public void Configure(EntityTypeBuilder<CustodyRequest> builder)
    {
        builder.HasOne<Parent>()
            .WithMany()
            .HasForeignKey(request => request.ParentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(request => request.Family)
            .WithMany()
            .HasForeignKey(request => request.FamilyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<CourtCase>()
            .WithMany()
            .HasForeignKey(request => request.CourtCaseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Custody>()
            .WithMany()
            .HasForeignKey(request => request.CustodyId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}