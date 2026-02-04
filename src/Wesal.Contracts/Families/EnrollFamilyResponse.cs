using Wesal.Contracts.Users;

namespace Wesal.Contracts.Families;

public record struct EnrollFamilyResponse(
    Guid FamilyId,
    UserCredentialResponse FatherCredential,
    UserCredentialResponse MotherCredential);