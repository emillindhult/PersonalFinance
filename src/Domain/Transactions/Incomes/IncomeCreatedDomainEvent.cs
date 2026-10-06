using Domain.Shared;

namespace Domain.Transactions.Incomes;

public sealed record IncomeCreatedDomainEvent(Guid BudgetId, decimal Amount) : IDomainEvent;