using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.CourtStaffs;
using Wesal.Domain.Entities.Users;

namespace Wesal.Infrastructure.CourtStaffs;

internal sealed class CourtStaffConfiguration : IEntityTypeConfiguration<CourtStaff>
{
    public void Configure(EntityTypeBuilder<CourtStaff> builder)
    {
        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<CourtStaff>(staff => staff.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(staff => staff.Court)
            .WithOne()
            .HasForeignKey<CourtStaff>(staff => staff.CourtId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(staff => staff.Email).IsUnique();
    }
}