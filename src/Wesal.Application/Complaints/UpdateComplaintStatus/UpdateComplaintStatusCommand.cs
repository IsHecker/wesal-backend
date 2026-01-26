using Wesal.Application.Messaging;

namespace Wesal.Application.Complaints.UpdateComplaintStatus;

public record struct UpdateComplaintStatusCommand(
    Guid StaffId,
    Guid ComplaintId,
    string Status,
    string ResolutionNotes) : ICommand;