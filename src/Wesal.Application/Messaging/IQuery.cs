using Wesal.Domain.Results;
using MediatR;

namespace Wesal.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>, IBaseRequest;