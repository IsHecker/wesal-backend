namespace Wesal.Application.Authentication;

public class CustomPolicies
{
    public const string SystemAdminOnly = nameof(SystemAdminOnly);
    public const string FamilyCourtAdminOnly = nameof(FamilyCourtAdminOnly);
    public const string CourtManagement = nameof(CourtManagement);
    public const string ParentsOnly = nameof(ParentsOnly);
    public const string CourtAndParents = nameof(CourtAndParents);
    public const string SchoolsOnly = nameof(SchoolsOnly);
    public const string VisitCenterStaffOnly = nameof(VisitCenterStaffOnly);
}