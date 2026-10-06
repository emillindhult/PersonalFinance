using Domain.CustomErrors;

namespace Domain.Categories;

public static class CategoryErrors
{
    public static Error NotFound =>
        Error.NotFound(
            "Categories.NotFound",
            "The category was not found."
        );

    public static Error AlreadyExists =>
        Error.Conflict(
            "Categories.AlreadyExists",
            "The category already exists."
        );
}
