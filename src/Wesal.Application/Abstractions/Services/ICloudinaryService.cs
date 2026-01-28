using Microsoft.AspNetCore.Http;
using Wesal.Domain.Results;

namespace Wesal.Application.Abstractions.Services;

public interface ICloudinaryService
{
    Task<Result<CloudinaryUploadResult>> UploadFileAsync(
        IFormFile file,
        CancellationToken cancellationToken = default);

    Task<Result> DeleteFileAsync(
        string publicId,
        CancellationToken cancellationToken = default);
}

public record struct CloudinaryUploadResult(string PublicId, string Url, long Bytes);