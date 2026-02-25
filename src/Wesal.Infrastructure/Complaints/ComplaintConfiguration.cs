using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Complaints;
using Wesal.Domain.Entities.Documents;

namespace Wesal.Infrastructure.Complaints;

internal sealed class ComplaintConfiguration : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasOne(complaint => complaint.Reporter)
            .WithMany()
            .HasForeignKey(complaint => complaint.ReporterId);

        builder.HasOne<Document>()
            .WithOne()
            .HasForeignKey<Complaint>(complaint => complaint.DocumentId);
    }
}