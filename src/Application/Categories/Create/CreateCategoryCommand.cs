using Application.Abstractions.Messaging;
using Domain.Categories;

namespace Application.Categories.Create;

public sealed record CreateCategoryCommand(string Name, CategoryType Type) : ICommand<Category>;