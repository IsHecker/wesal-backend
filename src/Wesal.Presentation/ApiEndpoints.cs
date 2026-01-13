namespace Wesal.Presentation;

public static class ApiEndpoints
{
    private const string ApiBase = "api";

    private const string ParentsBase = $"{ApiBase}/parents";
    private const string FamiliesBase = $"{ApiBase}/families";
    private const string CourtCasesBase = $"{ApiBase}/court-cases";
    private const string CustodiesBase = $"{ApiBase}/custodies";
    private const string visitationsBase = $"{ApiBase}/visitations";
    private const string PaymentsBase = $"{ApiBase}/payments";
    private const string NotificationsBase = $"{ApiBase}/notifications";

    public static class Parents
    {
        public const string Profile = $"{ParentsBase}/me";
    }

    public static class Families
    {
        public const string Enroll = FamiliesBase;
        public const string GetByParent = $"{FamiliesBase}/{{parentId:guid}}";
    }

    public static class CourtCases
    {
        public const string Create = CourtCasesBase;
        public const string ListByFamily = $"{CourtCasesBase}/{{familyId:guid}}";
    }

    public static class Custodies
    {
        public const string GetByFamily = $"{CustodiesBase}/{{familyId:guid}}";
    }

    public static class Visitations
    {
        public const string ListByFamily = $"{visitationsBase}/{{familyId:guid}}";
    }

    public static class Payments
    {
        public const string ListByFamily = $"{PaymentsBase}/{{familyId:guid}}";
        public const string MakeAlimony = PaymentsBase;
    }

    public static class Notifications
    {
        public const string ListByUser = $"{NotificationsBase}/me";
    }
}