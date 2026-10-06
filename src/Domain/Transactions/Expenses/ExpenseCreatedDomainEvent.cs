using Domain.Shared;

namespace Domain.Transactions.Expenses;

public sealed record ExpenseCreatedDomainEvent(Guid BudgetId, decimal Amount) : IDomainEvent;
