using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Shared;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetUserInfo;

public class GetUserInfoQueryHandler(IBudgetContext context) 
    : IQueryHandler<GetUserInfoQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserInfoQuery query,
        CancellationToken cancellationToken
    )
    {
        UserResponse? userResponse = await context.Users
            .Where(u => u.Id == query.Id)
            .Select(u => new UserResponse(u.FirstName, u.LastName, u.Email))
            .SingleOrDefaultAsync(cancellationToken);

        if (userResponse is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.Id));
        }

        return userResponse;
    }
}
