using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Repositories;
using Wesal.Application.ObligationAlerts;
using Wesal.Domain.Entities.Notifications;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.Parents;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.ObligationAlerts;

internal sealed class ObligationAlertService(
    IOptions<ObligationAlertOptions> alertOptions,
    IParentRepository parentRepository,
    WesalDbContext dbContext)
{
    public async Task CreateAlertAsync(
        Guid parentId,
        AlertType alertType,
        Guid relatedEntityId,
        string violationDescription)
    {
        var parent = await parentRepository.GetByIdAsync(parentId)
            ?? throw new InvalidOperationException("Parent not found while processing visitation.");

        parent.RecordViolation();
        parentRepository.Update(parent);

        if (parent.ViolationCount < alertOptions.Value.MaxViolationsCount)
            return;

        var alert = ObligationAlert.Create(
            parent.Id,
            relatedEntityId,
            parent.CourtId,
            alertType,
            $"Parent '{parent.FullName}' has {violationDescription}");

        await dbContext.ObligationAlerts.AddAsync(alert);

        await SendAlertNotificationAsync(parent);
    }

    private async Task SendAlertNotificationAsync(Parent parent)
    {
        // var notificationContent = $@"You missed your scheduled visitation on {visitation.Date} at {visitation.StartTime}.
        // This counts as violation #{parent.ViolationCount} of your obligations.
        // Please take note to avoid further violations and potential court notifications.";

        var notification = Notification.Create(
            parent.Id,
            "notificationContent",
            NotificationType.Alert);
    }
}