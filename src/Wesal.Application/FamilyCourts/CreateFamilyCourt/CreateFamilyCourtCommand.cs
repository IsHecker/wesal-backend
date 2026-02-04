using Wesal.Application.Messaging;
using Wesal.Contracts.Users;

namespace Wesal.Application.FamilyCourts.CreateFamilyCourt;

public record struct CreateFamilyCourtCommand(
    string Email,
    string Name,
    string Governorate,
    string Address,
    string ContactInfo) : ICommand<UserCredentialResponse>;