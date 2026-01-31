using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.Visitations.VisitationReminder;

internal sealed class VisitationReminderJobConfiguration(IOptions<VisitationReminderOptions> options)
    : IConfigureOptions<QuartzOptions>
{
    private readonly VisitationReminderOptions _options = options.Value;

    public void Configure(QuartzOptions options)
    {
        if (!_options.Enabled)
            return;

        string jobName = typeof(VisitationReminderJob).FullName!;

        options
            .AddJob<VisitationReminderJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithInterval(_options.RunInterval).RepeatForever()));
    }
}