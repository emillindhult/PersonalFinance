using Domain.Categories;

namespace API.Controllers.Categories;

public sealed record CategoryRequest(string Name, CategoryType Type);