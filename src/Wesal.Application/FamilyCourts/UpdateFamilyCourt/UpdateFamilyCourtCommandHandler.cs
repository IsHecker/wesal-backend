using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.FamilyCourts;
using Wesal.Domain.Results;

namespace Wesal.Application.FamilyCourts.UpdateFamilyCourt;

internal sealed class UpdateFamilyCourtCommandHandler(
    IFamilyCourtRepository courtRepository)
    : ICommandHandler<UpdateFamilyCourtCommand>
{
    public async Task<Result> Handle(UpdateFamilyCourtCommand request, CancellationToken cancellationToken)
    {
        var court = await courtRepository.GetByIdAsync(request.CourtId, cancellationToken);

        if (court is null)
            return FamilyCourtErrors.NotFound(request.CourtId);

        court.Update(
            request.Email,
            request.Name,
            request.Governorate,
            request.Address,
            request.ContactInfo);

        return Result.Success;
    }
}