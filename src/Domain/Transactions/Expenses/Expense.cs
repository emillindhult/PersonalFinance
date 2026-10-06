using Domain.Transactions;

namespace Domain.Transactions.Expenses;

public sealed class Expense : Transaction
{
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }

    public Expense(Guid accountId, Guid categoryId, decimal amount, DateTime date, string description)
        : base(amount, date, description)
    {
        AccountId = accountId;
        CategoryId = categoryId;
    }
}
