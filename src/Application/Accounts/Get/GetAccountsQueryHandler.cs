using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Accounts;
using Domain.CustomErrors;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Get;

internal sealed class GetAccountsQueryHandler(
    IBudgetContext context,
    IUserContext userContext
) : IQueryHandler<GetAccountsQuery, List<Account>>
{
    public async Task<Result<List<Account>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var user = await context.Users.SingleOrDefaultAsync(u => u.Id == userContext.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<List<Account>>(ValidationErrors.NotFound(nameof(user)));
        }

        var budget = await context.Budgets.SingleOrDefaultAsync(
            b => b.UserId == userContext.UserId,
            cancellationToken
        );

        if (budget is null)
        {
            return Result.Failure<List<Account>>(ValidationErrors.NotFound(nameof(budget)));
        }

        var accounts = await context.Accounts.Where(a => a.BudgetId == budget.Id).ToListAsync(cancellationToken);

        return accounts;
    }
}
