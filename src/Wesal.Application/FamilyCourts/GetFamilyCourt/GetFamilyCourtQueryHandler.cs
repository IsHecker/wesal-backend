using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Messaging;
using Wesal.Contracts.FamilyCourts;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

namespace Wesal.Application.FamilyCourts.GetFamilyCourt;

internal sealed class GetFamilyCourtQueryHandler(IWesalDbContext context)
    : IQueryHandler<GetFamilyCourtQuery, FamilyCourtResponse>
{
    public async Task<Result<FamilyCourtResponse>> Handle(
        GetFamilyCourtQuery request,
        CancellationToken cancellationToken)
    {
        var court = await context.FamilyCourts
            .FirstOrDefaultAsync(court => court.Id == request.CourtId, cancellationToken);

        if (court is null)
            return FamilyCourtErrors.NotFound(request.CourtId);

        return new FamilyCourtResponse(
            court.Id,
            court.Email,
            court.Name,
            court.Governorate,
            court.Address,
            court.ContactInfo);
    }
}