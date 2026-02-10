using Wesal.Application.Messaging;

namespace Wesal.Application.Parents.UpdateParentProfile;

public record struct UpdateParentProfileCommand(
    Guid CourtId,
    Guid ParentId,
    string? Email,
    string Phone,
    string Address,
    string? Job) : ICommand;