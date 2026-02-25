using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.ObligationAlerts;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Parents;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ObligationAlerts;

internal sealed class ObligationAlertService(
    IOptions<ObligationAlertOptions> alertOptions,
    INotificationService notificationService,
    WesalDbContext dbContext)
{
    private readonly ObligationAlertOptions alertOptions = alertOptions.Value;

    public async Task RecordViolationAsync(
        Guid parentId,
        ViolationType violationType,
        Guid relatedEntityId,
        string violationDescription,
        CancellationToken cancellationToken = default)
    {
        var parent = await GetParentByIdAsync(parentId, cancellationToken)
            ?? throw new InvalidOperationException("Parent not found while processing visitation.");

        parent.RecordViolation();

        var hasReachedMaxViolations = parent.ViolationCount >= alertOptions.MaxViolationsCount;

        var obligationAlert = ObligationAlert.Create(
            parent.CourtId,
            parent.Id,
            parent.FullName,
            relatedEntityId,
            violationType,
            hasReachedMaxViolations ? AlertStatus.Pending : AlertStatus.Drafted,
            violationDescription);

        await dbContext.ObligationAlerts.AddAsync(obligationAlert, cancellationToken);

        await SendViolationNotificationAsync(
            obligationAlert,
            violationDescription,
            hasReachedMaxViolations,
            cancellationToken);

        if (!hasReachedMaxViolations)
            return;

        await ActivateAlerts(parent.Id, cancellationToken);
        parent.ResetViolationCount();
    }

    private Task<Parent?> GetParentByIdAsync(Guid parentId, CancellationToken cancellationToken)
    {
        return dbContext.Parents
            .AsTracking()
            .FirstOrDefaultAsync(parent => parent.Id == parentId, cancellationToken);
    }

    private async Task ActivateAlerts(Guid parentId, CancellationToken cancellationToken)
    {
        var draftedAlerts = await GetDraftedAlertsByParentIdAsync(parentId, cancellationToken);

        foreach (var alert in draftedAlerts)
        {
            alert.Activate();
        }
    }

    private Task<List<ObligationAlert>> GetDraftedAlertsByParentIdAsync(
        Guid parentId,
        CancellationToken cancellationToken)
    {
        return dbContext.ObligationAlerts
            .AsTracking()
            .Where(alert => alert.Status == AlertStatus.Drafted && alert.ParentId == parentId)
            .ToListAsync(cancellationToken);
    }

    private async Task SendViolationNotificationAsync(
        ObligationAlert obligationAlert,
        string violationDescription,
        bool hasReachedMaxViolations,
        CancellationToken cancellationToken)
    {
        var notificationContent = BuildNotificationContent(violationDescription, hasReachedMaxViolations);

        await notificationService.SendNotificationsAsync(
            [NotificationTemplate.ObligationAlert(obligationAlert.ParentId, notificationContent)],
            cancellationToken: cancellationToken);
    }

    private string BuildNotificationContent(string violationDescription, bool hasReachedMaxViolations)
    {
        if (!hasReachedMaxViolations)
            return violationDescription;

        return $@"{violationDescription} You have reached the maximum number of violations ({alertOptions.MaxViolationsCount}). 
        The court has been notified and may take legal action.";
    }
}