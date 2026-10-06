using Domain.Transactions;

namespace Domain.Accounts;

public class Account
{
    public Guid Id { get; private set; }
    public Guid BudgetId { get; private set; }
    public string Name { get; private set; } = string.Empty;

    public Account(Guid budgetId, string name)
    {
        Id = Guid.NewGuid();
        BudgetId = budgetId;
        Name = name;
    }

    private Account() { }

    public void ChangeName(string name)
    {
        Name = name;
    }
}