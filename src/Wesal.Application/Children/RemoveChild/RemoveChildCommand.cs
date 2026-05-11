using Wesal.Application.Messaging;

namespace Wesal.Application.Children.RemoveChild;

public record struct RemoveChildCommand(
    Guid CourtId,
    Guid StaffId,
    Guid FamilyId,
    Guid ChildId) : ICommand;