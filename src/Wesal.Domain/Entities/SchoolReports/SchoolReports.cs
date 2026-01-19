using Wesal.Domain.DomainEvents;

namespace Wesal.Domain.Entities.SchoolReports;

public sealed class SchoolReport : Entity
{
    public Guid ChildId { get; private set; }
    public Guid SchoolId { get; private set; }

    public SchoolReportType ReportType { get; private set; }
    public string ReportUrl { get; private set; } = null!;
    public DateTime UploadedAt { get; private set; }

    private SchoolReport() { }

    public static SchoolReport Create(
        Guid childId,
        Guid schoolId,
        SchoolReportType reportType)
    {
        return new SchoolReport
        {
            ChildId = childId,
            SchoolId = schoolId,
            ReportType = reportType,
            ReportUrl = string.Empty,
            UploadedAt = DateTime.UtcNow,
        };
    }
}