using Wesal.Application.Messaging;

namespace Wesal.Application.Families.DeleteFamily;

public record struct DeleteFamilyCommand(Guid CourtId, Guid FamilyId) : ICommand;