using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.SchoolReports;

public static class SchoolReportErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("SchoolReport.NotFound", $"School report with ID '{id}' was not found");

    public static Error InvalidReportType(string reportType) =>
        Error.Validation("SchoolReport.InvalidReportType", $"Invalid report type: '{reportType}'");

    public static Error ChildNotInSchool() =>
        Error.Forbidden("SchoolReport.ChildNotInSchool", "This child is not enrolled in your school");
}