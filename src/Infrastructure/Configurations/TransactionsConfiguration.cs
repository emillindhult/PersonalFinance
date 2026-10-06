using Domain.Transactions;
using Domain.Transactions.Expenses;
using Domain.Transactions.Incomes;
using Domain.Transactions.Transfers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class TransactionsConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(t => t.Date)
            .IsRequired();

        builder.Property(t => t.Description)
            .HasMaxLength(200);

        builder
            .HasDiscriminator<string>("TransactionType")
            .HasValue<Income>("Income")
            .HasValue<Expense>("Expense")
            .HasValue<Transfer>("Transfer");
    }
}
