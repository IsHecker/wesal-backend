using Wesal.Domain.Entities.Alimonies;
using Wesal.Domain.Entities.PaymentsDue;

namespace Wesal.Domain.Entities.Notifications;

public static class NotificationTemplate
{
    public static Notification SchoolReportUploaded(Guid parentId, string childName) =>
        Notification.Create(
            parentId,
            "School Report Uploaded",
            $"A new school report for {childName} has been uploaded and is now available for review.",
            NotificationType.Update);


    public static Notification CustodyRequestApproved(Guid recipientId) =>
        Notification.Create(
            recipientId,
            "Custody Request Approved",
            "Your custody request has been approved by the court",
            NotificationType.Update);

    public static Notification CustodyRequestRejected(Guid recipientId, string reason) =>
        Notification.Create(
            recipientId,
            "Custody Request Rejected",
            $"Your custody request has been rejected. Reason: {reason}",
            NotificationType.Update);

    public static Notification PaymentDue(PaymentDue paymentDue) =>
        Notification.Create(
            paymentDue.Alimony.PayerId,
            "Payment Due",
            $"You have a payment of {paymentDue.Amount / 100:C} due on {paymentDue.DueDate:MMMM dd, yyyy}",
            NotificationType.Payment);

    public static Notification AlimoniesScheduled(Guid payerId) =>
        Notification.Create(
            payerId,
            "Alimonies Scheduled",
            $"Your alimony schedule for this month is now available",
            NotificationType.Schedule);

    public static Notification PaymentSuccess(Alimony alimony) =>
        Notification.Create(
            alimony.PayerId,
            "Payment Successful",
            $"Your payment of {alimony.Amount:C} for alimony has been successfully processed.",
            NotificationType.Payment);

    public static Notification PaymentFailed(Guid payerId, decimal amount) =>
        Notification.Create(
            payerId,
            "Payment Failed",
            $@"We couldn’t process your payment of {amount:C} for alimony. 
            Please try again later or update your payment method.",
            NotificationType.Payment);

    public static Notification AlimonyReadyToWithdraw(Alimony alimony) =>
        Notification.Create(
            alimony.RecipientId,
            "Alimony Available",
            $"You have received {alimony.Amount:C} in alimony. You can now withdraw it to your account.",
            NotificationType.Payment);

    public static Notification AlimonyWithdrawalPending(Guid recipientId, long amount) =>
        Notification.Create(
            recipientId,
            "Withdrawal Initiated",
            $@"Your withdrawal of {amount:C} has been initiated. 
        The funds should arrive in your bank shortly.",
            NotificationType.Payment);

    public static Notification AlimonyWithdrawalSuccess(Guid recipientId, long amount) =>
        Notification.Create(
            recipientId,
            "Withdrawal Successful",
            $@"You have successfully withdrawn {amount:C} from your alimony balance. 
            The funds should arrive in your bank shortly.",
            NotificationType.Payment);

    public static Notification AlimonyWithdrawalFailed(Guid recipientId, long amount) =>
        Notification.Create(
            recipientId,
            "Withdrawal Failed",
            $@"Your withdrawal of {amount:C} could not be completed. 
            Please check your bank details or try again later.",
            NotificationType.Payment);

    public static Notification VisitationScheduled(Guid parentId) =>
        Notification.Create(
            parentId,
            "Visitations Scheduled",
            $"Your visitation schedule for this month is now available",
            NotificationType.Schedule);

    public static Notification UpcomingVisitation(Guid parentId, DateTime startAt) =>
        Notification.Create(
            parentId,
            "Upcoming Visitation",
            $"You have a visitation scheduled for {startAt:MMMM dd, yyyy 'at' hh:mm tt}.",
            NotificationType.Reminder);


    public static Notification ObligationAlert(Guid parentId, string message) =>
        Notification.Create(
            parentId,
            "Obligation Alert",
            message,
            NotificationType.ObligationAlert);
}