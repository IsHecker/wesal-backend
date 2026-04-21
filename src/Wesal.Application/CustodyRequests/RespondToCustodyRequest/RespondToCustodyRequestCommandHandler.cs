using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Data;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Results;

namespace Wesal.Application.CustodyRequests.RespondToCustodyRequest;

internal sealed class RespondToCustodyRequestCommandHandler(
    ICustodyRequestRepository custodyRequestRepository,
    INotificationService notificationService,
    IObligationAlertService obligationAlertService,
    IOptions<CustodyRequestOptions> options,
    IWesalDbContext dbContext) : ICommandHandler<RespondToCustodyRequestCommand>
{
    private readonly CustodyRequestOptions options = options.Value;
    public async Task<Result> Handle(
        RespondToCustodyRequestCommand request,
        CancellationToken cancellationToken)
    {
        var custodyRequest = await custodyRequestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (custodyRequest is null)
            return CustodyRequestErrors.NotFound(request.RequestId);

        if (custodyRequest.CustodialParentId != request.ParentId)
            return CustodyRequestErrors.NotTheCustodialParent;

        custodyRequestRepository.Update(custodyRequest);

        var result = request.IsAccepted
            ? custodyRequest.Accept()
            : custodyRequest.Reject(request.ReasonNote!);

        if (result.IsFailure)
            return result.Error;

        if (request.IsAccepted)
        {
            await notificationService.SendNotificationsAsync(
                [NotificationTemplate.CustodyRequestAccepted(custodyRequest.NonCustodialParentId)],
                cancellationToken: cancellationToken);
        }
        else
        {
            await notificationService.SendNotificationsAsync(
                [NotificationTemplate.CustodyRequestRejected(custodyRequest.NonCustodialParentId, request.ReasonNote!)],
                cancellationToken: cancellationToken);

            await CheckForRepeatedRejectionsAsync(custodyRequest, cancellationToken);
        }

        return Result.Success;
    }

    private async Task CheckForRepeatedRejectionsAsync(CustodyRequest currentRequest, CancellationToken cancellationToken)
    {
        var maxRejections = options.MaxConsecutiveRejections;

        var lastApprovedAt = await dbContext.CustodyRequests
            .Where(cr => cr.FamilyId == currentRequest.FamilyId
                      && cr.CustodialParentId == currentRequest.CustodialParentId
                      && cr.Status == CustodyRequestStatus.Approved)
            .MaxAsync(cr => cr.RespondedAt, cancellationToken);

        var query = dbContext.CustodyRequests
            .Where(cr => cr.FamilyId == currentRequest.FamilyId
                      && cr.CustodialParentId == currentRequest.CustodialParentId
                      && cr.Status == CustodyRequestStatus.Rejected
                      && cr.Id != currentRequest.Id);

        if (lastApprovedAt.HasValue)
        {
            query = query.Where(cr => cr.RespondedAt > lastApprovedAt);
        }

        var previousRejectionsCount = await query.CountAsync(cancellationToken);
        var totalConsecutiveRejections = previousRejectionsCount + 1;

        if (totalConsecutiveRejections >= maxRejections)
        {
            await obligationAlertService.RecordViolationAsync(
                currentRequest.CustodialParentId,
                ViolationType.CustodyBreach,
                currentRequest.Id,
                "Repeated denial of stay requests",
                cancellationToken);
        }
    }
}
