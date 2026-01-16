using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.PaymentDues;
using Wesal.Domain.Entities.Payments;

namespace Wesal.Infrastructure.Payments;

internal sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasOne<Alimony>()
            .WithMany()
            .HasForeignKey(payment => payment.AlimonyId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne<PaymentDue>()
            .WithMany()
            .HasForeignKey(payment => payment.PaymentDueId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}