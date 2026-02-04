using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.CustodyRequests;

public sealed class CustodyRequest : Entity
{
    public Guid ParentId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid CourtCaseId { get; private set; }
    public Guid CustodyId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public string Reason { get; private set; } = null!;
    public CustodyRequestStatus Status { get; private set; }
    public string? DecisionNote { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    public Family Family { get; private set; } = null!;

    private CustodyRequest() { }

    public static CustodyRequest Create(
        Guid parentId,
        Guid familyId,
        Guid courtCaseId,
        Guid custodyId,
        DateOnly startDate,
        DateOnly endDate,
        string reason)
    {
        return new CustodyRequest
        {
            ParentId = parentId,
            FamilyId = familyId,
            CourtCaseId = courtCaseId,
            CustodyId = custodyId,
            StartDate = startDate,
            EndDate = endDate,
            Reason = reason,
            Status = CustodyRequestStatus.Pending
        };
    }

    public Result Approve(string decisionNote)
    {
        var result = StatusTransition
            .Validate(Status, CustodyRequestStatus.Pending, CustodyRequestStatus.Approved);

        if (result.IsFailure)
            return result.Error;

        Status = CustodyRequestStatus.Approved;
        DecisionNote = decisionNote;
        ProcessedAt = DateTime.UtcNow;

        return Result.Success;
    }

    public Result Reject(string decisionNote)
    {
        var result = StatusTransition
            .Validate(Status, CustodyRequestStatus.Pending, CustodyRequestStatus.Rejected);

        if (result.IsFailure)
            return result.Error;

        Status = CustodyRequestStatus.Rejected;
        DecisionNote = decisionNote;
        ProcessedAt = DateTime.UtcNow;

        return Result.Success;
    }
}