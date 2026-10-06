namespace Domain.Users;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public Guid UserId { get; private set; }
    public DateTime Expires { get; private set; }

    public User User { get; private set; } = null!;

    public RefreshToken(string token, Guid userId)
    {
        Id = Guid.NewGuid();
        Token = token;
        UserId = userId;
        Expires = DateTime.UtcNow.AddDays(7);
    }

    private RefreshToken() { }

    public void UpdateRefreshToken(string token)
    {
        Token = token;
        Expires = DateTime.UtcNow.AddDays(7);
    }
}
