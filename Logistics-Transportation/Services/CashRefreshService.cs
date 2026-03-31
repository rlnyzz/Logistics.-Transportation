using Logistics_Transportation.Services;
using StackExchange.Redis;
using System.Security.Cryptography;

public class RefreshTokenService : ICashRefreshTokenService
{
    private readonly IDatabase _redis;

    public RefreshTokenService(IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task<string> GenerateRefreshTokenAsync(string userId)
    {
        var token = GenerateToken();
        var hash = CreateSHA256(token);
        await _redis.StringSetAsync(
            $"refresh:{hash}",
            userId,
            TimeSpan.FromDays(15));

        return token;
    }

    public async Task<string?> ValidateRefreshTokenAsync(string refreshToken)
    {
        var hash = CreateSHA256(refreshToken);

        return await _redis.StringGetAsync($"refresh:{hash}");
    }

    public async Task RevokeAsync(string refreshToken)
    {
        var hash = CreateSHA256(refreshToken);

        await _redis.KeyDeleteAsync($"refresh:{hash}");
    }

    private string GenerateToken()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }

    private string CreateSHA256(string token)
    {
        using SHA256 hash = SHA256.Create();
        var bytes = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}