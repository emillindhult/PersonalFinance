using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Delete;

public class DeleteUserCommandHandler(IBudgetContext context, IUserContext userContext)
    : ICommandHandler<DeleteUserCommand>
{
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await context.Users
            .SingleOrDefaultAsync(u => u.Id == userContext.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(userContext.UserId));
        }

        await context.RefreshTokens
            .Where(rt => rt.UserId == userContext.UserId)
            .ExecuteDeleteAsync(cancellationToken);

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
