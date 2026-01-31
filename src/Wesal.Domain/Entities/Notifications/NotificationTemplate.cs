using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.ObligationAlerts;
using Wesal.Domain.Entities.PaymentsDue;
using Wesal.Domain.Entities.Visitations;
using Wesal.Domain.Entities.VisitationSchedules;

namespace Wesal.Domain.Entities.Notifications;

public static class NotificationTemplate
{
    public static Notification CaseUpdate(Guid recipientId, Guid relatedEntityId, string entityType, string caseName) =>
        Notification.Create(
            recipientId,
            relatedEntityId,
            entityType,
            "Case Update",
            $"Your case '{caseName}' has been updated",
            NotificationType.CaseUpdate);

    public static Notification CourtDecision(Guid recipientId, Guid relatedEntityId, string entityType, string decisionSummary) =>
        Notification.Create(
            recipientId,
            relatedEntityId,
            entityType,
            "Court Decision Issued",
            $"A decision has been made in your case: {decisionSummary}",
            NotificationType.CourtDecision);

    public static Notification CustodyRequestApproved(Guid recipientId, Guid relatedEntityId, string entityType) =>
        Notification.Create(
            recipientId,
            relatedEntityId,
            entityType,
            "Custody Request Approved",
            "Your custody request has been approved by the court",
            NotificationType.CustodyRequestApproved);

    public static Notification CustodyRequestRejected(Guid recipientId, Guid relatedEntityId, string entityType, string reason) =>
        Notification.Create(
            recipientId,
            relatedEntityId,
            entityType,
            "Custody Request Rejected",
            $"Your custody request has been rejected. Reason: {reason}",
            NotificationType.CustodyRequestRejected);

    public static Notification PaymentDue(PaymentDue paymentDue) =>
        Notification.Create(
            paymentDue.Alimony.PayerId,
            paymentDue.Id,
            nameof(PaymentDue),
            "Payment Due",
            $"You have a payment of {paymentDue.Amount / 100:C} due on {paymentDue.DueDate:MMMM dd, yyyy}",
            NotificationType.PaymentDue);

    public static Notification AlimoniesScheduled(Alimony alimony) =>
        Notification.Create(
            alimony.PayerId,
            alimony.Id,
            nameof(Alimony),
            "Alimonies Scheduled",
            $"Your alimony schedule for this month is now available",
            NotificationType.AlimonyScheduled);

    public static Notification PaymentReceived(Guid recipientId, Guid relatedEntityId, string entityType, decimal amount) =>
        Notification.Create(
            recipientId,
            relatedEntityId,
            entityType,
            "Payment Received",
            $"Payment of {amount:C} has been received and processed",
            NotificationType.PaymentReceived);

    public static Notification VisitationScheduled(VisitationSchedule schedule) =>
        Notification.Create(
            schedule.ParentId,
            schedule.Id,
            nameof(VisitationSchedule),
            "Visitations Scheduled",
            $"Your visitation schedule for this month is now available",
            NotificationType.VisitationScheduled);

    public static Notification UpcomingVisitation(Visitation visitation) =>
        Notification.Create(
            visitation.ParentId,
            visitation.Id,
            nameof(Visitation),
            "Upcoming Visitation",
            $"You have a visitation scheduled for {visitation.StartAt:MMMM dd, yyyy 'at' hh:mm tt}.",
            NotificationType.Reminder);


    public static Notification ObligationAlert(ObligationAlert obligationAlert, string message) =>
        Notification.Create(
            obligationAlert.ParentId,
            obligationAlert.Id,
            nameof(ObligationAlert),
            "Obligation Alert",
            message,
            NotificationType.ObligationAlert);
}