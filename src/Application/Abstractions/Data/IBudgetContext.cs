using Domain.Accounts;
using Domain.Budgets;
using Domain.Categories;
using Domain.Transactions;
using Domain.Transactions.Expenses;
using Domain.Transactions.Incomes;
using Domain.Transactions.Transfers;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IBudgetContext
{
    DbSet<User> Users { get; }
    DbSet<Budget> Budgets { get; }
    DbSet<Account> Accounts { get; }
    DbSet<Category> Categories { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<Income> Incomes { get; }
    DbSet<Expense> Expenses { get; }
    DbSet<Transfer> Transfers { get; }
    DbSet<RefreshToken> RefreshTokens { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
