using Application.Abstractions.Messaging;
using Domain.Accounts;

namespace Application.Accounts.Update;

public sealed record ChangeAccountNameCommand(
    Guid Id, 
    string Name
) : ICommand<Account>;