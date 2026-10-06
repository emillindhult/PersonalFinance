using Application.Abstractions.Messaging;
using Domain.Accounts;

namespace Application.Accounts.GetById;

public sealed record GetAccountByIdQuery(Guid Id) : IQuery<Account>;