using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

namespace Wesal.Application.FamilyCourts.DeleteFamilyCourt;

internal sealed class DeleteFamilyCourtCommandHandler(
    IFamilyCourtRepository courtRepository,
    ICourtStaffRepository staffRepository,
    IFamilyRepository familyRepository)
    : ICommandHandler<DeleteFamilyCourtCommand>
{
    public async Task<Result> Handle(DeleteFamilyCourtCommand request, CancellationToken cancellationToken)
    {
        var court = await courtRepository.GetByIdAsync(request.CourtId, cancellationToken);

        if (court is null)
            return FamilyCourtErrors.NotFound(request.CourtId);

        var hasStaff = await staffRepository.Query().AnyAsync(s => s.CourtId == request.CourtId, cancellationToken);
        if (hasStaff)
            return FamilyCourtErrors.HasReferences;

        var hasFamilies = await familyRepository.Query().AnyAsync(f => f.CourtId == request.CourtId, cancellationToken);
        if (hasFamilies)
            return FamilyCourtErrors.HasReferences;

        courtRepository.Delete(court);

        return Result.Success;
    }
}