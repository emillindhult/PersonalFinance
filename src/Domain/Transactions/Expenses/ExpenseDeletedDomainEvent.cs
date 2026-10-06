using Domain.Shared;

namespace Domain.Transactions.Expenses;

public sealed record ExpenseDeletedDomainEvent(Guid BudgetId, decimal Amount) : IDomainEvent;