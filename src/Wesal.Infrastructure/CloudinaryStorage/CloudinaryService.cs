using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wesal.Application.Abstractions.Services;
using Wesal.Domain.Entities.Documents;
using Wesal.Domain.Results;

namespace Wesal.Infrastructure.CloudinaryStorage;

internal sealed class CloudinaryService : ICloudinaryService
{
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<CloudinaryService> _logger;

    public CloudinaryService(
        IOptions<CloudinaryOptions> options,
        ILogger<CloudinaryService> logger)
    {
        _logger = logger;

        var account = new Account(
            options.Value.CloudName,
            options.Value.ApiKey,
            options.Value.ApiSecret);

        _cloudinary = new Cloudinary(account);
        _cloudinary.Api.Secure = true;
    }

    public async Task<Result<CloudinaryUploadResult>> UploadFileAsync(
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await using var stream = file.OpenReadStream();

            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                PublicId = $"wesal/documents/{Guid.NewGuid()}",
                UseFilename = true,
                UniqueFilename = true,
                Overwrite = false,
                AssetFolder = "Wesal/documents"
            };

            var uploadResult = await _cloudinary.UploadLargeAsync(
                uploadParams,
                cancellationToken: cancellationToken);

            if (uploadResult.Error is not null)
            {
                _logger.LogError(
                    "Cloudinary upload failed: {ErrorMessage}",
                    uploadResult.Error.Message);

                return DocumentErrors.UploadFailed;
            }

            return new CloudinaryUploadResult(
                uploadResult.PublicId,
                uploadResult.SecureUrl.ToString(),
                uploadResult.Bytes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while uploading file to Cloudinary");
            return DocumentErrors.UploadFailed;
        }
    }

    public async Task<Result> DeleteFileAsync(
        string publicId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw,
                Invalidate = true
            };

            var deleteResult = await _cloudinary.DestroyAsync(deleteParams);

            if (deleteResult.Result != "ok" && deleteResult.Result != "not found")
            {
                _logger.LogError(
                    "Cloudinary delete failed: {Result}",
                    deleteResult.Result);

                return DocumentErrors.DeleteFailed;
            }

            return Result.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception occurred while deleting file from Cloudinary");
            return Result.Failure(DocumentErrors.DeleteFailed);
        }
    }
}