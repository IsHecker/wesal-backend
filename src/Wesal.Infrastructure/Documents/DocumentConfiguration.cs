using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Entities.Users;

namespace Wesal.Infrastructure.Documents;

internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(document => document.Id);

        builder.Property(document => document.FileName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(document => document.MimeType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(document => document.CloudinaryPublicId)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(document => document.CloudinaryUrl)
            .IsRequired()
            .HasMaxLength(1000);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(document => document.UploadedBy)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(document => document.UploadedBy);
        builder.HasIndex(document => document.UploadedAt);
        builder.HasIndex(document => document.CloudinaryPublicId);
    }
}
