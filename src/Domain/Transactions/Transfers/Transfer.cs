using Domain.Transactions;

namespace Domain.Transactions.Transfers;

public sealed class Transfer : Transaction
{
    public Guid FromAccountId { get; private set; }
    public Guid ToAccountId { get; private set; }

    public Transfer(Guid fromAccountId, Guid toAccountId, decimal amount, DateTime date, string description)
        : base(amount, date, description)
    {
        if (fromAccountId == toAccountId)
            throw new ArgumentException("Cannot transfer to the same account.");

        FromAccountId = fromAccountId;
        ToAccountId = toAccountId;
    }
}
