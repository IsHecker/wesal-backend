using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.Visitations.VisitationSessionsGeneration;

internal sealed class GenerateVisitationSessionsJobConfiguration(IOptions<GenerateVisitationSessionsOptions> options)
    : IConfigureOptions<QuartzOptions>
{
    private readonly GenerateVisitationSessionsOptions _options = options.Value;

    public void Configure(QuartzOptions options)
    {
        if (!_options.Enabled)
            return;

        string jobName = typeof(GenerateVisitationSessionsJob).FullName!;

        options
            .AddJob<GenerateVisitationSessionsJob>(configure => configure.WithIdentity(jobName))
            .AddTrigger(configure =>
                configure
                    .ForJob(jobName)
                    .WithSimpleSchedule(schedule =>
                        schedule.WithInterval(_options.RunInterval).RepeatForever()));
    }
}