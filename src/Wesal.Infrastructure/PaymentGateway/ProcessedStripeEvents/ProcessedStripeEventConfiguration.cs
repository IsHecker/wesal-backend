using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wesal.Infrastructure.PaymentGateway.ProcessedStripeEvents;

public class ProcessedStripeEventConfiguration : IEntityTypeConfiguration<ProcessedStripeEvent>
{
    public void Configure(EntityTypeBuilder<ProcessedStripeEvent> builder)
    {
        builder.HasKey(x => x.EventId);
        builder.HasIndex(x => x.ProcessedAt);
    }
}