using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.CourtStaffs;

namespace Wesal.Infrastructure.CourtStaffs;

internal sealed class StaffWorkloadConfiguration : IEntityTypeConfiguration<StaffWorkload>
{
    public void Configure(EntityTypeBuilder<StaffWorkload> builder)
    {
        builder.Property(e => e.Id).ValueGeneratedNever();

        builder.HasIndex(w => new { w.CourtStaffId, w.Type }).IsUnique();
    }
}