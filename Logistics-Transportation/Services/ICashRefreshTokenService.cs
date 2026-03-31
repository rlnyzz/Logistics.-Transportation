namespace Logistics_Transportation.Services
{
    public interface ICashRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(string userId);
        Task<string?> ValidateRefreshTokenAsync(string refreshToken);
        Task RevokeAsync(string refreshToken);
    }
}
