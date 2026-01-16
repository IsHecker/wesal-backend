using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.ObligationAlerts.OverdueAlimonyPaymentsDetection;

internal sealed class DetectOverdueAlimonyPaymentsJobConfiguration(IOptions<DetectOverdueAlimonyPaymentsOptions> options)
    : IConfigureOptions<QuartzOptions>
{
    private readonly DetectOverdueAlimonyPaymentsOptions _options = options.Value;

    public void Configure(QuartzOptions options)
    {
        if (!_options.Enabled)
            return;

        string jobName = typeof(DetectOverdueAlimonyPaymentsJob).FullName!;

        options
            .AddJob<DetectOverdueAlimonyPaymentsJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithInterval(_options.RunInterval).RepeatForever()));
    }
}