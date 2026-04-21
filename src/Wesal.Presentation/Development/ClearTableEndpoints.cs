using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Application.Abstractions.Data;
using Wesal.Presentation.Endpoints;
using Wesal.Presentation.Extensions;

namespace Wesal.Presentation.Development;

internal sealed class ClearTableEndpoints : IEndpoint
{
    private static readonly string[] Tables =
    [
        "Users",
        "FamilyCourts",
        "CourtStaffs",
        "VisitCenterStaffs",
        "Parents",
        "Schools",
        "SchoolReports",
        "Families",
        "Children",
        "CourtCases",
        "Custodies",
        "CustodyRequests",
        "Visitations",
        "VisitationSchedules",
        "VisitationLocations",
        "Alimonies",
        "PaymentsDue",
        "Payments",
        "Complaints",
        "ObligationAlerts",
        "ComplianceMetrics",
        "Notifications",
        "Documents",
        "UserDevices",
        "ProcessedStripeEvents",
        "SystemAdmin"
    ];

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        foreach (var table in Tables)
        {
            var route = table.ToLowerInvariant();
            app.MapDelete($"/api/dev/clear/{route}", async (IWesalDbContext context) =>
            {
                try
                {
                    await ClearTableManually(context, table);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.Problem(
                        detail: ex.Message,
                        title: $"Failed to clear table {table} manually.",
                        statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithTags(Tags.Development)
            .WithOpenApiName($"Clear{table}")
            .WithSummary($"Deletes all records from the {table} table with manual cascade.")
            .AllowAnonymous();
        }
    }

    private static async Task ClearTableManually(IWesalDbContext context, string table)
    {
        // Global Leaf-to-Root order that is safe for partial and full deletions
        var globalOrder = new[]
        {
            "Payments",
            "PaymentsDue",
            "Alimonies",
            "Visitations",
            "VisitationSchedules",
            "CustodyRequests",
            "Custodies",
            "SchoolReports",
            "Children",
            "Complaints",
            "CourtCases",
            "ComplianceMetrics",
            "ObligationAlerts",
            "Notifications",
            "Families",
            "Parents",
            "CourtStaffs",
            "VisitCenterStaffs",
            "VisitationLocations",
            "FamilyCourts",
            "SystemAdmin",
            "Schools",
            "UserDevices",
            "Documents",
            "Users",
            "ProcessedStripeEvents"
        };

        // For each target table, we identifies its subset of the global order
        // which includes all tables that could transitively depend on it.
        var targets = table switch
        {
            "Users" => globalOrder,
            "FamilyCourts" =>
            [
                "Payments", "PaymentsDue", "Alimonies", "Visitations", "VisitationSchedules",
                "CustodyRequests", "Custodies", "SchoolReports", "Children", "Complaints",
                "CourtCases", "ComplianceMetrics", "ObligationAlerts", "Notifications",
                "Families", "Parents", "CourtStaffs", "VisitCenterStaffs", "VisitationLocations",
                "FamilyCourts"
            ],
            "Parents" =>
            [
                "Payments", "PaymentsDue", "Alimonies", "Visitations", "VisitationSchedules",
                "CustodyRequests", "Custodies", "SchoolReports", "Children", "Complaints",
                "CourtCases", "ComplianceMetrics", "ObligationAlerts", "Notifications",
                "Families", "Parents"
            ],
            "Families" =>
            [
                "Payments", "PaymentsDue", "Alimonies", "Visitations", "VisitationSchedules",
                "CustodyRequests", "Custodies", "SchoolReports", "Children", "Complaints",
                "CourtCases", "ComplianceMetrics", "Families"
            ],
            "CourtCases" =>
            [
                "Payments", "PaymentsDue", "Alimonies", "Visitations", "VisitationSchedules",
                "CustodyRequests", "Custodies", "CourtCases"
            ],
            "VisitationLocations" =>
            [
                "Visitations", "VisitationSchedules", "VisitCenterStaffs", "VisitationLocations"
            ],
            "Schools" =>
            [
                "SchoolReports", "Children", "Schools"
            ],
            "Children" =>
            [
                "SchoolReports", "Children"
            ],
            "Alimonies" =>
            [
                "Payments", "PaymentsDue", "Alimonies"
            ],
            "PaymentsDue" =>
            [
                "Payments", "PaymentsDue"
            ],
            "VisitationSchedules" =>
            [
                "Visitations", "VisitationSchedules"
            ],
            "Custodies" =>
            [
                "CustodyRequests", "Custodies"
            ],
            "VisitCenterStaffs" =>
            [
                "Visitations", "VisitCenterStaffs"
            ],
            "Payments" =>
            [
                "Payments", "PaymentsDue" // Circular reference handling: clear both
            ],
            "Documents" =>
            [
                "SchoolReports", "Complaints", "CourtCases", "Documents"
            ],
            _ => [table]
        };

        foreach (var t in targets)
        {
            await context.ExecuteSqlAsync($"DELETE FROM [{t}]");
        }
    }
}