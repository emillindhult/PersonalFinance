using Application.Abstractions.Messaging;
using Domain.Accounts;

namespace Application.Accounts.Create;

public sealed record CreateAccountCommand(string Name) : ICommand<Account>;