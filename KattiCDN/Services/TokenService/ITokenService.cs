using Microsoft.AspNetCore.Mvc;

namespace KattiCDN.Services.TokenService
{
    public interface ITokenService
    {
        public string CreateAccessToken(string username, int user_id);
        public Task<string> CreateRefreshToken(int user_id);
        public Task<int> ValidateRefreshToken(string refreshToken);
        public Task DeleteRefreshTokens(int user_id);
        public string CreateApiKey(int user_id);
        public void DeleteApiKey(int user_id);
    }
}
