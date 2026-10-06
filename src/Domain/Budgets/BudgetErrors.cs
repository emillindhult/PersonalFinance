using Domain.CustomErrors;

namespace Domain.Budgets;

public static class BudgetErrors
{
    public static Error NotFound =>
        Error.NotFound(
            "Budgets.NotFound",
            $"The budget was not found."
        );

    public static Error Unauthorized() =>
        Error.Failure(
            "Budgets.Unauthorized",
            "You are not authorized to perform this action."
        );
}
