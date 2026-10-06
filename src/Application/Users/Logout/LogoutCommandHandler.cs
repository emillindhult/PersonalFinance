using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.CustomErrors;
using Domain.Shared;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Logout;

internal sealed class LogoutCommandHandler(
    IBudgetContext context,
    IUserContext userContext)
    : ICommandHandler<LogoutCommand>
{
    public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var result = await context.RefreshTokens
            .Where(rt => rt.UserId == userContext.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        return Result.Success();
    }
}
