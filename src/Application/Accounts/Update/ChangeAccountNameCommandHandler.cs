using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Accounts;
using Domain.CustomErrors;
using Domain.Shared;
using Domain.Budgets;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Update;

internal sealed class ChangeAccountNameCommandHandler(
    IBudgetContext context,
    IUserContext userContext
) : ICommandHandler<ChangeAccountNameCommand, Account>
{
    public async Task<Result<Account>> Handle(ChangeAccountNameCommand request, CancellationToken cancellationToken)
    {
        Guid? budgetId = await context.Budgets
            .Where(b => b.UserId == userContext.UserId)
            .Select(b => (Guid?)b.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (budgetId is null)
        {
            return Result.Failure<Account>(BudgetErrors.NotFound);
        }

        Account? account = await context.Accounts
            .SingleOrDefaultAsync(a => a.Id == request.Id && a.BudgetId == budgetId.Value, cancellationToken);

        if (account is null)
        {
            return Result.Failure<Account>(ValidationErrors.NotFound(nameof(account)));
        }

        account.ChangeName(request.Name);

        context.Accounts.Update(account);
        await context.SaveChangesAsync(cancellationToken);

        return account;
    }
}
