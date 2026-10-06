using Domain.CustomErrors;

namespace Domain.Transactions.Expenses;

public static class ExpenseErrors
{
    public static Error NotFound(Guid expenseId) =>
        Error.NotFound(
            "Expenses.NotFound",
            $"The expense with Id: {expenseId} was not found."
        );

    public static Error NoFound =>
        Error.NotFound(
            "Expenses.NoFound",
            $"No Expenses found."
        );
}
