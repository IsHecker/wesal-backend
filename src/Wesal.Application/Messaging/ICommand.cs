using Wesal.Domain.Results;
using MediatR;

namespace Wesal.Application.Messaging;

public interface ICommand : IRequest<Result>, IBaseRequest;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseRequest;

public interface IBaseRequest;