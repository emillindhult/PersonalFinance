using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Users;
using Domain.Shared;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.RefreshTokens;

internal sealed class RefreshTokenCommandHandler(
    IBudgetContext context,
    ITokenProvider tokenProvider,
    IRefreshTokenHasher refreshTokenHasher
) : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken
    )
    {
        string tokenHash = refreshTokenHasher.Hash(request.RefreshToken);

        RefreshToken? refreshToken = await context
            .RefreshTokens.Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == tokenHash, cancellationToken);

        if (refreshToken is null || refreshToken.Expires < DateTime.UtcNow)
        {
            return Result.Failure<TokenResponse>(UserErrors.InvalidRefreshToken);
        }

        string accessToken = tokenProvider.GenerateToken(refreshToken.User);
        string plainRefreshToken = tokenProvider.GenerateRefreshToken();

        refreshToken.UpdateRefreshToken(refreshTokenHasher.Hash(plainRefreshToken));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(new TokenResponse(accessToken, plainRefreshToken));
    }
}
