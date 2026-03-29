using Wesal.Domain.Common;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Families;
using Wesal.Domain.Results;

namespace Wesal.Domain.Entities.CustodyRequests;

public sealed class CustodyRequest : Entity
{
    public Guid NonCustodialParentId { get; private set; }
    public Guid FamilyId { get; private set; }
    public Guid CourtCaseId { get; private set; }
    public Guid CustodyId { get; private set; }
    public Guid CustodialParentId { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public string Reason { get; private set; } = null!;
    public CustodyRequestStatus Status { get; private set; }
    public string? ReasonNote { get; private set; }
    public DateTime? RespondedAt { get; private set; }

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
            NonCustodialParentId = parentId,
            FamilyId = familyId,
            CourtCaseId = courtCaseId,
            CustodyId = custodyId,
            StartDate = startDate,
            EndDate = endDate,
            Reason = reason,
            Status = CustodyRequestStatus.Pending
        };
    }

    public void ForwardTo(Guid custodialParentId)
    {
        CustodialParentId = custodialParentId;
    }

    public Result Accept()
    {
        var result = StatusTransition
            .Validate(Status, CustodyRequestStatus.Pending, CustodyRequestStatus.Approved);

        if (result.IsFailure)
            return result.Error;

        Status = CustodyRequestStatus.Approved;
        RespondedAt = EgyptTime.Now;

        return Result.Success;
    }

    public Result Reject(string reasonNote)
    {
        var result = StatusTransition
            .Validate(Status, CustodyRequestStatus.Pending, CustodyRequestStatus.Rejected);

        if (result.IsFailure)
            return result.Error;

        Status = CustodyRequestStatus.Rejected;
        ReasonNote = reasonNote;
        RespondedAt = EgyptTime.Now;

        return Result.Success;
    }
}