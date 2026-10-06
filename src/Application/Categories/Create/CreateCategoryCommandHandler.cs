using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Budgets;
using Domain.Categories;
using Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Create;

internal sealed class CreateCategoryCommandHandler(
    IBudgetContext context,
    IUserContext userContext
) : ICommandHandler<CreateCategoryCommand, Category>
{
    public async Task<Result<Category>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Guid? budgetId = await context.Budgets
            .Where(b => b.UserId == userContext.UserId)
            .Select(b => (Guid?)b.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (budgetId is null)
        {
            return Result.Failure<Category>(BudgetErrors.NotFound);
        }

        bool exists = await context.Categories.AnyAsync(
            c => c.BudgetId == budgetId && 
            c.Name == request.Name && 
            c.Type == request.Type, 
            cancellationToken);

        if (exists)
        {
            return Result.Failure<Category>(CategoryErrors.AlreadyExists);
        }

        var category = new Category(budgetId.Value, request.Name, request.Type);

        await context.Categories.AddAsync(category, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return category;
    }
}
