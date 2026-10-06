using Domain.Shared;

namespace Domain.Transactions.Expenses;

public sealed record ExpenseUpdatedDomainEvent(Guid BudgetId, decimal OldAmount, decimal NewAmount)
    : IDomainEvent;
