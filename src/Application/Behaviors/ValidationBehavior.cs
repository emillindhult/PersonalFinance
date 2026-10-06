using System.Reflection;
using Domain.CustomErrors;
using Domain.Shared;
using FluentValidation;
using MediatR;

namespace Application.Behaviors;

internal sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var errors = validationResults
            .SelectMany(r => r.Errors)
            .Where(e => e is not null)
            .Select(e => e.ErrorMessage)
            .ToArray();

        if (errors.Length == 0)
        {
            return await next();
        }

        var error = Error.Validation(
            "Validation.Failed",
            string.Join("; ", errors)
        );

        return CreateValidationResult<TResponse>(error);
    }

    private static TResult CreateValidationResult<TResult>(Error error)
    {
        Type resultType = typeof(TResult);

        if (resultType == typeof(Result))
        {
            return (TResult)(object)Result.Failure(error);
        }

        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            Type valueType = resultType.GetGenericArguments()[0];

            MethodInfo failureMethod = typeof(Result)
                .GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(m =>
                    m.Name == nameof(Result.Failure)
                    && m.IsGenericMethodDefinition
                    && m.GetParameters().Length == 1
                )
                .MakeGenericMethod(valueType);

            return (TResult)failureMethod.Invoke(null, [error])!;
        }

        throw new ValidationException(error.Description);
    }
}
