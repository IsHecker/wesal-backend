namespace Wesal.Contracts.Families;

public record struct EnrollFamilyResponse(
    Guid FamilyId,
    string FatherTemporaryPassword,
    string MotherTemporaryPassword);