using Microsoft.Extensions.Options;
using Quartz;

namespace Wesal.Infrastructure.ObligationAlerts.OverdueAlimonyPaymentsDetection;

[DisallowConcurrentExecution]
internal sealed class DetectOverdueAlimonyPaymentsJob(
    IOptions<DetectOverdueAlimonyPaymentsOptions> options) : IJob
{
    private readonly DetectOverdueAlimonyPaymentsOptions _options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
    }
}