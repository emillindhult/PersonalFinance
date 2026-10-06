namespace Domain.CustomErrors;

public static class ExceptionError
{
    public static Error Exception(string description) =>
        Error.Problem(
            "Exceptions.Error",
            $"An unexpected error occurred: {description}."
        );
}
