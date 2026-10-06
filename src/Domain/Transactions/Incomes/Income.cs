using Domain.Transactions;

namespace Domain.Transactions.Incomes;

public sealed class Income : Transaction
{
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }

    public Income(Guid accountId, Guid categoryId, decimal amount, DateTime date, string description)
        : base(amount, date, description)
    {
        AccountId = accountId;
        CategoryId = categoryId;
    }
}
