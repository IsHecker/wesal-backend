using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Entities.Notifications;

namespace Wesal.Application.CustodyRequests.CreateCustodyRequest;

internal sealed class CreateCustodyRequestCommandHandler(
    IFamilyRepository familyRepository,
    ICustodyRepository custodyRepository,
    ICustodyRequestRepository custodyRequestRepository,
    INotificationService notificationService)
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

        if (custody.CustodialParentId == request.ParentId)
            return CustodyRequestErrors.AlreadyCustodian;

        var alreadyPending = await custodyRequestRepository
            .HasPendingByParentAndFamilyAsync(request.ParentId, family.Id, cancellationToken);
                
        if (alreadyPending)
            return CustodyRequestErrors.AlreadyPending;

        var custodyRequest = CustodyRequest.Create(
            request.ParentId,
            family.Id,
            custody.CourtCaseId,
            custody.Id,
            request.StartDate,
            request.EndDate,
            request.Reason);
            
        custodyRequest.ForwardTo(custody.CustodialParentId);

        await custodyRequestRepository.AddAsync(custodyRequest, cancellationToken);
        
        await notificationService.SendNotificationsAsync(
            [NotificationTemplate.CustodyRequestReceived(custodialParentId: custody.CustodialParentId)],
            cancellationToken: cancellationToken);

        return custodyRequest.Id;
    }
}