using Wesal.Application.Data;
using Wesal.Domain.Entities.VisitationLocations;

namespace Wesal.Application.Abstractions.Repositories;

public interface IVisitationLocationRepository : IRepository<VisitationLocation>
{
    Task<bool> ExistsByNameAndGovernorateAsync(
        string name,
        string governorate,
        CancellationToken cancellationToken = default);
}