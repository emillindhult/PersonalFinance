using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Accounts;
using Domain.Budgets;
using Domain.CustomErrors;
using Domain.Shared;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Accounts.Create;

internal sealed class CreateAccountCommandHandler(
    IBudgetContext context, 
    IUserContext userContext
) 
    : ICommandHandler<CreateAccountCommand, Account>
{
    public async Task<Result<Account>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        Guid? budgetId = await context.Budgets
            .Where(b => b.UserId == userContext.UserId)
            .Select(b => (Guid?)b.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (budgetId is null)
        {
            return Result.Failure<Account>(BudgetErrors.NotFound);
        }

        var account = new Account(budgetId.Value, request.Name);

        await context.Accounts.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return account;
    }
}
