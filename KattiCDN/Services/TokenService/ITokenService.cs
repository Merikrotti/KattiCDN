using Microsoft.AspNetCore.Mvc;

namespace KattiCDN.Services.TokenService
{
    public interface ITokenService
    {
        public string CreateAccessToken(string username, int user_id);
        public string CreateRefreshToken(string username, int user_id);
        public void DeleteRefreshTokens(int user_id);
        public string CreateApiKey(int user_id);
        public void DeleteApiKey(int user_id);
    }
}
