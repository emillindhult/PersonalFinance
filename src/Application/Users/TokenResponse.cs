namespace Application.Users;

public sealed record TokenResponse(string AccessToken, string RefreshToken);