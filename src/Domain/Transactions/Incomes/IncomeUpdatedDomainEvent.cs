using Domain.Shared;

namespace Domain.Transactions.Incomes;

public sealed record IncomeUpdatedDomainEvent(Guid BudgetId, decimal OldAmount, decimal NewAmount) 
    : IDomainEvent;