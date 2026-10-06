using Domain.Budgets;
using Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Type)
            .IsRequired();

        builder.Property(c => c.BudgetId)
            .IsRequired();

        builder.HasOne<Budget>()
            .WithMany()
            .HasForeignKey(c => c.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(c => new { c.BudgetId, c.Name, c.Type })
            .IsUnique();
    }
}
