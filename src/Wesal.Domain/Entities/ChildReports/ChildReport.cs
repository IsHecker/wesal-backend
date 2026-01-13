using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Children;
using Wesal.Domain.Entities.Schools;

namespace Wesal.Domain.Entities.ChildReports;

public sealed class ChildReport : Entity
{
    public Guid ChildId { get; private set; }
    public Guid SchoolId { get; private set; }

    public string ReportUrl { get; private set; } = null!;
    public DateTime UploadedAt { get; private set; }

    public Child Child { get; private set; } = null!;
    public School School { get; private set; } = null!;

    private ChildReport() { }

    public static ChildReport Create(Guid childId, Guid schoolId, string documentUrl, DateTime uploadedAt)
    {
        return new ChildReport
        {
            ChildId = childId,
            SchoolId = schoolId,
            ReportUrl = documentUrl,
            UploadedAt = uploadedAt,
        };
    }
}