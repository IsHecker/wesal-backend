using Wesal.Application.Messaging;

namespace Wesal.Application.Children.AddChild;

public record struct AddChildCommand(
    Guid CourtId,
    Guid FamilyId,
    Guid? SchoolId,
    string FullName,
    DateOnly BirthDate,
    string Gender) : ICommand<Guid>;