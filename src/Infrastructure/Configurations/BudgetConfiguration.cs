using Domain.Budgets;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        builder.ToTable("Budgets");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.UserId)
            .IsRequired();

        builder
            .HasIndex(b => b.UserId)
            .IsUnique();

        builder.Navigation(b => b.Accounts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(b => b.Accounts)
            .WithOne()
            .HasForeignKey(a => a.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
