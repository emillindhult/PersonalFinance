using Application.Abstractions.Messaging;
using Domain.Accounts;

namespace Application.Accounts.Get;

public sealed record GetAccountsQuery : IQuery<List<Account>>;