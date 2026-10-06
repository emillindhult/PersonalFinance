using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Budgets;
using Domain.Shared;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Register;

public sealed class RegisterUserCommandHandler(
    IBudgetContext context,
    IPasswordHasher passwordHasher
) : ICommandHandler<RegisterUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken
    )
    {
        bool userExists = await context.Users
            .AnyAsync(e => e.Email == command.Email, cancellationToken);

        if (userExists)
        {
            return Result.Failure<Guid>(UserErrors.EmailExists);
        }

        var newUser = new User(
            command.FirstName, 
            command.LastName, 
            command.Email, 
            passwordHasher.Hash(command.Password)
        );

        var userBudget = new Budget(newUser.Id);

        context.Users.Add(newUser);
        context.Budgets.Add(userBudget);
        await context.SaveChangesAsync(cancellationToken);

        return newUser.Id;
    }
}
