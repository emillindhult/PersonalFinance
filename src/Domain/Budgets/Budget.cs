using Domain.Accounts;
using Domain.Shared;

namespace Domain.Budgets;

public class Budget : Entity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }

    private readonly List<Account> _accounts = [];
    public IReadOnlyCollection<Account> Accounts => _accounts;

    public Budget(Guid userId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
    }

    private Budget() 
    {
    }

    public void AddAccount(Account account)
    {
        _accounts.Add(account);
    }
}