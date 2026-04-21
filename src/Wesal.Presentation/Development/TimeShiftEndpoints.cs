using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Wesal.Domain.Common;
using Wesal.Presentation.Endpoints;

namespace Wesal.Presentation.Development;

internal sealed class TimeShiftEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/dev")
            .WithTags(Tags.Development)
            .AllowAnonymous();

        group.MapPost("time/shift", (ShiftTimeRequest request) =>
        {
            var targetUtc = request.TargetTime.ToUtc();
            EgyptTime.Offset = targetUtc - DateTime.UtcNow;

            return Results.Ok(new
            {
                CurrentTime = EgyptTime.Now,
                Offset = EgyptTime.Offset.ToString()
            });
        })
        .WithSummary("Shifts the system time to the specified date and time.");

        group.MapDelete("time/reset", () =>
        {
            EgyptTime.Offset = TimeSpan.Zero;
            return Results.Ok(new
            {
                CurrentTime = EgyptTime.Now,
                Offset = EgyptTime.Offset.ToString()
            });
        })
        .WithSummary("Resets the system time offset to zero.");

        // group.MapPost("jobs/trigger-all", async (ISchedulerFactory schedulerFactory) =>
        // {
        //     var scheduler = await schedulerFactory.GetScheduler();
        //     var jobKeys = await scheduler.GetJobKeys(Quartz.Impl.Matchers.GroupMatcher<JobKey>.AnyGroup());

        //     foreach (var jobKey in jobKeys)
        //     {
        //         await scheduler.TriggerJob(jobKey);
        //     }

        //     return Results.Ok(new { Message = $"Triggered {jobKeys.Count} jobs." });
        // })
        // .WithSummary("Manually triggers all scheduled Quartz jobs.");
    }

    private sealed record ShiftTimeRequest(DateTime TargetTime);
}