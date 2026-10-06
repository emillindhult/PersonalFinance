using Domain.Shared;

namespace Domain.Transactions.Incomes;

public sealed record IncomeDeletedDomainEvent(Guid BudgetId, decimal Amount) : IDomainEvent;