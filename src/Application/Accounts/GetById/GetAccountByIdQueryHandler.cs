using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Accounts;
using Domain.Budgets;
using Domain.CustomErrors;
using Domain.Shared;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.GetById;

internal sealed class GetAccountByIdQueryHandler(
    IBudgetContext context,
    IUserContext userContext
) : IQueryHandler<GetAccountByIdQuery, Account>
{
    public async Task<Result<Account>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        User? user = await context.Users.SingleOrDefaultAsync(u => u.Id == userContext.UserId, cancellationToken);

        if (user is null)
        {
            return Result.Failure<Account>(UserErrors.Unauthorized);
        }

        Budget? budget = await context.Budgets.SingleOrDefaultAsync(b => b.UserId == user.Id, cancellationToken);

        if (budget is null)
        {
            return Result.Failure<Account>(BudgetErrors.NotFound);
        }

        Account? account = await context.Accounts.SingleOrDefaultAsync(a => a.Id == request.Id && a.BudgetId == budget.Id, cancellationToken);

        if (account is null)
        {
            return Result.Failure<Account>(ValidationErrors.NotFound(nameof(account)));
        }

        return account;
    }
}
