using Domain.CustomErrors;

namespace Domain.Users;

public static class UserErrors
{
    public static Error NotFound(Guid userId) =>
        Error.NotFound(
            "Users.NotFound",
            $"The user with Id: {userId} was not found."
        );

    public static Error Unauthorized =>
        Error.Failure(
            "Users.Unauthorized",
            "You are not authorized to perform this action."
        );

    public static Error NotFoundByEmail =>
        Error.NotFound(
            "Users.NotFoundByEmail",
            "The user with the specified email was not found."
        );

    public static Error InvalidCredentials =>
        Error.Failure(
            "Users.InvalidCredentials",
            "Invalid email or password."
        );

    public static Error EmailExists =>
        Error.Conflict(
            "Users.EmailNotUnique",
            "The provided email is not unique."
        );

    public static Error NoRefreshTokens => 
        Error.NotFound(
            "Users.NoRefreshTokens",
            "No refresh tokens found for the user."
        );

    public static Error InvalidRefreshToken =>
        Error.Failure(
            "Users.InvalidRefreshToken",
            "The refresh token is invalid or has expired."
        );
}
