using Wesal.Application.Messaging;

namespace Wesal.Application.FamilyCourts.UpdateFamilyCourt;

public sealed record UpdateFamilyCourtCommand(
    Guid CourtId,
    string Email,
    string Name,
    string Governorate,
    string Address,
    string ContactInfo) : ICommand;