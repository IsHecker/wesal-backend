using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Entities.SchoolReports;
using Wesal.Domain.Entities.Schools;

namespace Wesal.Infrastructure.SchoolReports;

internal sealed class SchoolReportConfiguration : IEntityTypeConfiguration<SchoolReport>
{
    public void Configure(EntityTypeBuilder<SchoolReport> builder)
    {
        builder.HasOne<Child>()
            .WithOne()
            .HasForeignKey<SchoolReport>(report => report.ChildId);

        builder.HasOne<School>()
            .WithOne()
            .HasForeignKey<SchoolReport>(report => report.SchoolId);

        builder.HasOne<Document>()
            .WithOne()
            .HasForeignKey<SchoolReport>(report => report.DocumentId);
    }
}