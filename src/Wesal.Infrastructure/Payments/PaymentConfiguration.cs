using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Payments;
using Wesal.Domain.Entities.PaymentsDue;

namespace Wesal.Infrastructure.Payments;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasOne<PaymentDue>()
            .WithMany()
            .HasForeignKey(payment => payment.PaymentDueId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}