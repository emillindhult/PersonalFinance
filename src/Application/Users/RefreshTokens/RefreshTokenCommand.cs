using Application.Abstractions.Messaging;
using Application.Users;

namespace Application.Users.RefreshTokens;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<TokenResponse>;