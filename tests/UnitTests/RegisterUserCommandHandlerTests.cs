using Application.Abstractions.Authentication;
using Application.Users.Register;
using Domain.Users;
using FluentAssertions;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace PersonalFinance.Tests;

public sealed class RegisterUserCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenEmailAlreadyExists_ShouldReturnFailure()
    {
        using var context = CreateInMemoryBudgetContext();

        context.Users.Add(
            new User(
                "Emil",
                "Lindhult",
                "emil@test.com",
                "hashedpass123"
            )
        );

        await context.SaveChangesAsync();

        var passwordHasher = Substitute.For<IPasswordHasher>();

        var handler = new RegisterUserCommandHandler(context, passwordHasher);

        var command = new RegisterUserCommand(
            "emil@test.com",
            "Emil",
            "Lindhult",
            "otherpass12"
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(UserErrors.EmailExists);

        context.Users.Count().Should().Be(1);

        passwordHasher.DidNotReceive().Hash(Arg.Any<string>());
    }

    [Fact]
    public async Task Handle_WhenEmailIsUnique_ShouldCreateUserAndBudget()
    {
        using var context = CreateInMemoryBudgetContext();

        var passwordHasher = Substitute.For<IPasswordHasher>();

        passwordHasher
            .Hash("password")
            .Returns("hashed-password");

        var handler = new RegisterUserCommandHandler(context, passwordHasher);

        var command = new RegisterUserCommand(
            "emil@test.com",
            "Emil",
            "Lindhult",
            "password"
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var user = await context.Users.SingleAsync();

        user.Email.Should().Be("emil@test.com");
        user.PasswordHash.Should().Be("hashed-password");

        var budget = await context.Budgets.SingleAsync();

        budget.UserId.Should().Be(user.Id);

        passwordHasher
            .Received(1)
            .Hash("password");
    }

    private static BudgetContext CreateInMemoryBudgetContext()
    {
        var options = new DbContextOptionsBuilder<BudgetContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var publisher = Substitute.For<IPublisher>();

        return new BudgetContext(options, publisher);
    }
}
