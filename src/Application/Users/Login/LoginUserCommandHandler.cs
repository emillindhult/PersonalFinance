using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Login;

internal sealed class LoginUserCommandHandler(
    IBudgetContext context,
    ITokenProvider tokenProvider,
    IPasswordHasher passwordHasher,
    IRefreshTokenHasher refreshTokenHasher
) : ICommandHandler<LoginUserCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(
        LoginUserCommand command,
        CancellationToken cancellationToken
    )
    {
        User? user = await context
            .Users.AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<TokenResponse>(UserErrors.InvalidCredentials);
        }

        bool verified = passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!verified)
        {
            return Result.Failure<TokenResponse>(UserErrors.InvalidCredentials);
        }

        string accessToken = tokenProvider.GenerateToken(user);
        string plainRefreshToken = tokenProvider.GenerateRefreshToken();

        var refreshToken = new RefreshToken(
            refreshTokenHasher.Hash(plainRefreshToken),
            user.Id
        );

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new TokenResponse(accessToken, plainRefreshToken));
    }
}
