using System;

namespace Wesal.Domain.Entities.FamilyCourts
{
    public class CourtSettings
    {
        public Guid Id { get; set; }
        public Guid FamilyCourtId { get; set; }
        public int VisitationGracePeriodMinutes { get; set; } = 15;
        public int AlimonyReminderDaysBefore { get; set; } = 7;
        public int VisitationReminderHoursBefore { get; set; } = 24;
        public string CustodyRequestWorkflow { get; set; } = null!;
        public decimal MinPaymentAmount { get; set; }
        public decimal MaxPaymentAmount { get; set; }
        public string PaymentDueDateRule { get; set; } = null!;
        public int MaxConcurrentVisitations { get; set; }
        public int CaseClosureMonthsOfInactivity { get; set; }
        public string SchoolCredentialFormat { get; set; } = null!;
        public int VisitationGenerationMonthsAhead { get; set; }
        public string VisitationCancellationPolicy { get; set; } = null!;
        public string AllowedDocumentTypes { get; set; } = null!;
        public int DocumentSizeLimitMb { get; set; }
        public string DocumentAccessRoles { get; set; } = null!;
        public string RequiredSchoolReportTypes { get; set; } = null!;
        public int SchoolReportSubmissionDeadlineDays { get; set; }
    }
}