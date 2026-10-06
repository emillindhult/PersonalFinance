using Domain.CustomErrors;

namespace Domain.Transactions.Incomes;

public static class IncomeErrors
{
    public static Error NotFound(Guid incomeId) =>
        Error.NotFound(
            "Incomes.NotFound",
            $"The income with Id: {incomeId} was not found."
        );
}
