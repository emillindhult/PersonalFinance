using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Authentication;

namespace Infrastructure.Authentication;

internal sealed class RefreshTokenHasher : IRefreshTokenHasher
{
    public string Hash(string token)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(hash);
    }
}
