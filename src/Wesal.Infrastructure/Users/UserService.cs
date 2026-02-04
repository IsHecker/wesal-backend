using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Wesal.Application.Abstractions.Services;
using Wesal.Application.Data;
using Wesal.Application.Users;
using Wesal.Domain.DomainEvents;
using Wesal.Domain.Entities.Users;
using Wesal.Infrastructure.Database;

namespace Wesal.Infrastructure.Users;

internal sealed class UserService(
    WesalDbContext dbContext,
    IRepository<User> userRepository,
    IPasswordHasher<User> passwordHasher) : IUserService
{
    public async Task<UserResult> CreateAsync(
        string role,
        CancellationToken cancellationToken = default)
    {
        var password = GeneratePassword();
        var hashedPassword = passwordHasher.HashPassword(null!, password);
        var user = User.Create(role, hashedPassword);

        await userRepository.AddAsync(user, cancellationToken);

        return new(user, password);
    }

    public async Task<bool> ExistsByEmailAsync<TEntity>(
        string email,
        CancellationToken cancellationToken) where TEntity : Entity
    {
        return await dbContext.Set<TEntity>().AnyAsync(
            e => EF.Property<string>(e, "Email") == email,
            cancellationToken);
    }

    private static string GeneratePassword()
    {
        const int base64Length = 8;
        const int keyBytes = base64Length / 4 * 3;

        Span<byte> bytes = stackalloc byte[keyBytes];
        RandomNumberGenerator.Fill(bytes);

        Span<char> base64Chars = stackalloc char[base64Length];
        Convert.TryToBase64Chars(bytes, base64Chars, out _);

        base64Chars.Replace('+', RandomLowerCase());
        base64Chars.Replace('=', RandomLowerCase());
        base64Chars.Replace('/', RandomNumber());

        return new string(base64Chars);

        static char RandomLowerCase() => (char)RandomNumberGenerator.GetInt32(97, 123);
        static char RandomNumber() => (char)RandomNumberGenerator.GetInt32(48, 58);
    }
}