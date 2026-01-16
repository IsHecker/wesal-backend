using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Domain.Entities.Payments;

namespace Wesal.Infrastructure.PaymentDues;

internal sealed class PaymentDueConfiguration : IEntityTypeConfiguration<PaymentDue>
{
    public void Configure(EntityTypeBuilder<PaymentDue> builder)
    {
        builder.HasOne<Alimony>()
            .WithMany()
            .HasForeignKey(paymentDue => paymentDue.AlimonyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Family>()
            .WithMany()
            .HasForeignKey(paymentDue => paymentDue.FamilyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<Payment>()
            .WithOne()
            .HasForeignKey<PaymentDue>(paymentDue => paymentDue.PaymentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}