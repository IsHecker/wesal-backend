using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Application.CustodyRequests.CreateCustodyRequest;

internal sealed class CreateCustodyRequestCommandHandler(
    IFamilyRepository familyRepository,
    ICustodyRepository custodyRepository,
    IRepository<CustodyRequest> custodyRequestRepository)
    : ICommandHandler<CreateCustodyRequestCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        CreateCustodyRequestCommand request,
        CancellationToken cancellationToken)
    {
        var family = await familyRepository.GetByParentIdAsync(request.ParentId, cancellationToken);
        if (family is null)
            return FamilyErrors.NotFoundByParent(request.ParentId);

        var custody = await custodyRepository.GetByFamilyIdAsync(family.Id, cancellationToken);
        if (custody is null)
            return CustodyRequestErrors.NoCustodyDecision;

        if (custody.CustodianId == request.ParentId)
            return CustodyRequestErrors.AlreadyCustodian;

        var custodyRequest = CustodyRequest.Create(
            request.ParentId,
            family.Id,
            custody.CourtCaseId,
            custody.Id,
            request.StartDate,
            request.EndDate,
            request.Reason);

        await custodyRequestRepository.AddAsync(custodyRequest, cancellationToken);

        return custodyRequest.Id;
    }
}
