using System.Transactions;
using MediatR;
using Wesal.Application.Data;
using Wesal.Domain.Results;

namespace Wesal.Application.Behaviours;

public class UnitOfWorkBehaviour<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var scopeOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted
        };

        using (var scope = new TransactionScope(
            TransactionScopeOption.Required,
            scopeOptions,
            TransactionScopeAsyncFlowOption.Enabled))
        {
            var response = await next();

            if (response.IsFailure)
                return response;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
            return response;
        }
    }
}