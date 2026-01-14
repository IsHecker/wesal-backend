using Wesal.Application.Abstractions.Repositories;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Infrastructure.Data;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ObligationAlerts;

internal sealed class ObligationAlertRepository(WesalDbContext context)
    : Repository<ObligationAlert>(context), IObligationAlertRepository
{
}