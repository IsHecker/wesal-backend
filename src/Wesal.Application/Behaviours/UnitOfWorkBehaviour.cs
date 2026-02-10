using System.Transactions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Wesal.Application.Data;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Results;

namespace Wesal.Application.Behaviours;

public class UnitOfWorkBehaviour<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : Result
{
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
            var response = await next(cancellationToken);

            if (response.IsFailure)
                return response;

            UpdateEntityDates(unitOfWork);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            scope.Complete();
            return response;
        }
    }

    private static void UpdateEntityDates(IUnitOfWork unitOfWork)
    {
        var context = (DbContext)unitOfWork;

        var entities = context
            .ChangeTracker
            .Entries<Entity>()
            .Where(entry => entry.State == EntityState.Modified)
            .Select(entry => entry.Entity);

        foreach (var entity in entities)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}