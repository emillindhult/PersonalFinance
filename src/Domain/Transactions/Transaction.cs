namespace Domain.Transactions;

public abstract class Transaction
{
    public Guid Id { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string Description { get; private set; } = string.Empty;

    protected Transaction(decimal amount, DateTime date, string description)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.");

        Id = Guid.NewGuid();
        Amount = amount;
        Date = date;
        Description = description;
    }

    private Transaction() { }
}