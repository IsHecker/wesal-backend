namespace Wesal.Presentation;

internal static class ApiEndpoints
{
    private const string ApiBase = "api";

    private const string CourtsBase = $"{ApiBase}/courts";
    private const string ParentsBase = $"{ApiBase}/parents";
    private const string SchoolsBase = $"{ApiBase}/schools";
    private const string SchoolReportsBase = $"{ApiBase}/school-reports";
    private const string FamiliesBase = $"{ApiBase}/families";
    private const string CourtCasesBase = $"{ApiBase}/court-cases";
    private const string CustodiesBase = $"{ApiBase}/custodies";
    private const string CustodyRequestsBase = $"{ApiBase}/custody-requests";
    private const string VisitationSchedulesBase = $"{ApiBase}/visitation-schedules";
    private const string visitationsBase = $"{ApiBase}/visitations";
    private const string visitationLocationsBase = $"{ApiBase}/visitation-locations";
    private const string PaymentsDueBase = $"{ApiBase}/payments-due";
    private const string PaymentsBase = $"{ApiBase}/payments";
    private const string AlimoniesBase = $"{ApiBase}/alimonies";
    private const string ObligationAlertsBase = $"{ApiBase}/obligation-alerts";
    private const string NotificationsBase = $"{ApiBase}/notifications";
    private const string ComplaintsBase = $"{ApiBase}/complaints";
    private const string DocumentsBase = $"{ApiBase}/documents";

    internal static class Courts
    {
        public const string Me = $"{CourtsBase}/me";
    }

    internal static class Parents
    {
        public const string Profile = $"{ParentsBase}/me";
    }

    internal static class Families
    {
        public const string GetById = $"{FamiliesBase}/{{familyId:guid}}";
        public const string ListByCourt = $"{Courts.Me}/families";

        public const string Enroll = FamiliesBase;
        public const string GetByParent = FamiliesBase;
    }

    internal static class Children
    {
        public const string ListBySchool = $"{SchoolsBase}/me/children";
    }

    internal static class Schools
    {
        public const string GetById = $"{SchoolsBase}/{{schoolId:guid}}";
        public const string List = SchoolsBase;
        public const string Register = SchoolsBase;
    }

    internal static class SchoolReports
    {
        public const string ListByChild = $"{SchoolReportsBase}/{{childId:guid}}";
        public const string Upload = SchoolReportsBase;
    }

    internal static class CourtCases
    {
        public const string Create = CourtCasesBase;
        public const string ListByFamily = $"{CourtCasesBase}/{{familyId:guid}}";
    }

    internal static class Custodies
    {
        public const string GetByFamily = $"{CustodiesBase}/{{familyId:guid}}";
        public const string Create = CustodiesBase;
    }

    internal static class CustodyRequests
    {
        public const string ListByCourt = CustodyRequestsBase;
        public const string Process = $"{CustodyRequestsBase}/{{requestId:guid}}/process";
        public const string Create = CustodyRequestsBase;
    }

    internal static class VisitationSchedules
    {
        public const string ListByFamily = $"{Families.GetById}/visitation-schedules";
        public const string Create = VisitationSchedulesBase;
    }

    internal static class Visitations
    {
        public const string GetById = $"{visitationsBase}/{{visitationId:guid}}";

        public const string List = visitationsBase;
        public const string CheckIn = $"{GetById}/check-in";
        public const string Complete = $"{GetById}/complete";
    }

    internal static class VisitationLocations
    {
        public const string List = visitationLocationsBase;
        public const string Create = visitationLocationsBase;
        public const string Update = $"{visitationLocationsBase}/{{locationId:guid}}";
    }

    internal static class PaymentsDue
    {
        public const string GetById = $"{PaymentsDueBase}/{{paymetDueId:guid}}";
        public const string ListByFamily = $"{Families.GetById}/payments-due";
    }

    internal static class Payments
    {
        public const string ListByPaymentDue = $"{PaymentsDue.GetById}/payments";
        public const string MakeAlimony = $"{PaymentsDue.GetById}/payments";
    }

    internal static class Notifications
    {
        public const string ListByUser = $"{NotificationsBase}/me";
    }

    internal static class Alimonies
    {
        public const string Create = AlimoniesBase;
    }

    internal static class Complaints
    {
        public const string ListByCourt = $"{Courts.Me}/complaints";
        public const string UpdateStatus = $"{ComplaintsBase}/{{complaintId:guid}}/status";
        public const string Create = ComplaintsBase;
    }

    internal static class ObligationAlerts
    {
        public const string List = ObligationAlertsBase;
        public const string UpdateStatus = $"{ObligationAlertsBase}/{{alertId:guid}}/status";
    }

    internal static class Documents
    {
        public const string Upload = DocumentsBase;
        public const string GetById = $"{DocumentsBase}/{{documentId:guid}}";
        public const string Delete = GetById;
    }
}