using Wesal.Domain.Results;

namespace Wesal.Application.Exceptions;

public sealed class WesalException : Exception
{
    public WesalException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}
