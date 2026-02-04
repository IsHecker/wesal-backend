using Microsoft.AspNetCore.Identity;
using Wesal.Application.Data;
using Wesal.Application.Messaging;
using Wesal.Domain.Entities.Users;
using Wesal.Domain.Results;

namespace Wesal.Application.Users.ChangePassword;

internal sealed class ChangePasswordCommandHandler(
    IRepository<User> userRepository,
    IPasswordHasher<User> passwordHasher)
    : ICommandHandler<ChangePasswordCommand>
{
    public async Task<Result> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return UserErrors.NotFound(request.UserId);

        var verifyResult = passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.OldPassword);

        if (verifyResult == PasswordVerificationResult.Failed)
            return UserErrors.IncorrectOldPassword;

        var newHashedPassword = passwordHasher.HashPassword(user, request.NewPassword);

        user.ChangePassword(newHashedPassword);
        userRepository.Update(user);

        return Result.Success;
    }
}