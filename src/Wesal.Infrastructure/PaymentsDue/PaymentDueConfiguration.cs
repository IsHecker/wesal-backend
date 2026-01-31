using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;

namespace Wesal.Infrastructure.PaymentsDue;

internal sealed class PaymentDueConfiguration : IEntityTypeConfiguration<PaymentDue>
{
    public void Configure(EntityTypeBuilder<PaymentDue> builder)
    {
        builder.HasOne(paymentDue => paymentDue.Alimony)
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