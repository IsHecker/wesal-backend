namespace Wesal.Application.Authentication;

public class CustomPolicies
{
    public const string SystemAdminOnly = nameof(SystemAdminOnly);
    public const string FamilyCourtAdminOnly = nameof(FamilyCourtAdminOnly);
    public const string CourtAdminOnly = nameof(CourtAdminOnly);
    public const string SettlementSpecialistOnly = nameof(SettlementSpecialistOnly);
    public const string CaseClerkOnly = nameof(CaseClerkOnly);
    public const string ComplianceMonitorOnly = nameof(ComplianceMonitorOnly);
    public const string ParentsOnly = nameof(ParentsOnly);
    public const string CourtAndParents = nameof(CourtAndParents);
    public const string SchoolsOnly = nameof(SchoolsOnly);
    public const string VisitCenterStaffOnly = nameof(VisitCenterStaffOnly);
    public const string CourtStaffOnly = nameof(CourtStaffOnly);
}