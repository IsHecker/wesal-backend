using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.ObligationAlerts.MissedVisitationsDetection;

internal sealed class DetectMissedVisitationsJobConfiguration(IOptions<DetectMissedVisitationsOptions> options)
    : IConfigureOptions<QuartzOptions>
{
    private readonly DetectMissedVisitationsOptions _options = options.Value;

    public void Configure(QuartzOptions options)
    {
        if (!_options.Enabled)
            return;

        string jobName = typeof(DetectMissedVisitationsJob).FullName!;

        options
            .AddJob<DetectMissedVisitationsJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithInterval(_options.RunInterval).RepeatForever()));
    }
}