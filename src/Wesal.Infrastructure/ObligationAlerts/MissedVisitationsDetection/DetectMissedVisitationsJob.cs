using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.ObligationAlerts.MissedVisitationsDetection;

[DisallowConcurrentExecution]
internal sealed class DetectMissedVisitationsJob(
    IOptions<DetectMissedVisitationsOptions> options) : IJob
{
    private readonly DetectMissedVisitationsOptions _options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
    }
}