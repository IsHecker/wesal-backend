using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Entities.VisitationLocations;
using Wesal.Domain.Entities.VisitCenterStaffs;

namespace Wesal.Infrastructure.VisitCenterStaffs;

internal sealed class VisitCenterStaffConfiguration : IEntityTypeConfiguration<VisitCenterStaff>
{
    public void Configure(EntityTypeBuilder<VisitCenterStaff> builder)
    {
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<VisitCenterStaff>(staff => staff.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        builder.HasOne<VisitationLocation>()
            .WithOne()
            .HasForeignKey<VisitCenterStaff>(staff => staff.LocationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(staff => staff.Email).IsUnique();
    }
}