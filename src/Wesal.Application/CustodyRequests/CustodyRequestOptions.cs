namespace Wesal.Application.CustodyRequests;

public sealed class CustodyRequestOptions
{
    public const string SectionName = "CustodyRequests";

    public int MaxConsecutiveRejections { get; init; }
}