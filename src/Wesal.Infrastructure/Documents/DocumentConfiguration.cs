using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Documents;

namespace Wesal.Infrastructure.Documents;

internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("Documents");

        builder.HasKey(document => document.Id);

        builder.Property(document => document.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(document => document.FileSizeBytes)
            .IsRequired();

        builder.Property(document => document.MimeType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(document => document.CloudinaryPublicId)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(document => document.CloudinaryUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(document => document.UploadedBy)
            .IsRequired();

        builder.Property(document => document.UploadedAt)
            .IsRequired();

        builder.HasIndex(document => document.UploadedBy);
        builder.HasIndex(document => document.UploadedAt);
        builder.HasIndex(document => document.CloudinaryPublicId);
    }
}
