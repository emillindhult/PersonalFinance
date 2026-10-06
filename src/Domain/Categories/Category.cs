namespace Domain.Categories;

public class Category
{
    public Guid Id { get; private set; }
    public Guid BudgetId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public CategoryType Type { get; private set; }

    public Category(Guid budgetId, string name, CategoryType type)
    {
        Id = Guid.NewGuid();
        BudgetId = budgetId;
        Name = name.Trim();
        Type = type;
    }

    private Category() { }
}
