using Microsoft.EntityFrameworkCore;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.PaymentGateway.ProcessedStripeEvents;

internal sealed class ProcessedStripeEventRepository(WesalDbContext dbContext)
{
    public Task<bool> IsEventProcessedAsync(string eventId, CancellationToken cancellationToken = default)
    {
        return dbContext.ProcessedStripeEvents.AnyAsync(e => e.EventId == eventId, cancellationToken);
    }

    public async Task MarkEventAsProcessedAsync(string eventId, CancellationToken cancellationToken = default)
    {
        await dbContext.ProcessedStripeEvents
            .AddAsync(new ProcessedStripeEvent(eventId), cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}