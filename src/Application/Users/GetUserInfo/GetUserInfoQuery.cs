using Application.Abstractions.Messaging;

namespace Application.Users.GetUserInfo;

public sealed record GetUserInfoQuery(Guid Id) : IQuery<UserResponse>;
