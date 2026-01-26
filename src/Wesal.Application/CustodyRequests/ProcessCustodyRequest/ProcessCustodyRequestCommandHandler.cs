using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.CustodyRequests;
using Wesal.Domain.Results;

namespace Wesal.Application.CustodyRequests.ProcessCustodyRequest;

internal sealed class ProcessCustodyRequestCommandHandler(
    IRepository<CustodyRequest> custodyRequestRepository)
    : ICommandHandler<ProcessCustodyRequestCommand>
{
    public async Task<Result> Handle(
        ProcessCustodyRequestCommand request,
        CancellationToken cancellationToken)
    {
        var custodyRequest = await custodyRequestRepository.GetByIdAsync(request.RequestId, cancellationToken);
        if (custodyRequest is null)
            return CustodyRequestErrors.NotFound(request.RequestId);

        custodyRequestRepository.Update(custodyRequest);

        return request.IsApproved
        ? custodyRequest.Approve(request.DecisionNote)
        : custodyRequest.Reject(request.DecisionNote);
    }
}