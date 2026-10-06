namespace Domain.CustomErrors;

public static class ValidationErrors
{
    public static Error NullOrWhiteSpace =>
        Error.Failure(
            "Validation.NullOrWhiteSpace",
            "String was null or empty."
        );

    public static Error EndDateBeforeStartDate =>
        Error.Failure(
            "Validation.EndDateBeforeStartDate",
            "The end date cannot be earlier than the start date."
        );

    public static Error LessThanZero =>
        Error.Failure(
            "Validation.LessThanZero",
            "Value cannot be less than zero."
        );

    public static Error NotFound(string name) =>
        Error.NotFound(
            "Validation.NotFound",
            $"{name} was not found."
        );
}
