using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KattiCDN.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly CdnDatabase _context;

        public TokenService(IConfiguration configuration, CdnDatabase context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string CreateAccessToken(string username, int user_id)
        {
            var tokenHandler = new JwtSecurityToken();
            var token = _configuration["Authentication:Schemes:Bearer:Token"];

            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, username));
            claims.Add(new Claim("uid", user_id.ToString()));

            var accesstoken = TokenGenerator(token ?? "", 0.1, claims);

            return accesstoken;
        }

        public string CreateRefreshToken(string username, int user_id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Generates JWT key to use
        /// </summary>
        /// <param name="token">Secret key</param>
        /// <param name="expirationHours">How many hours until the JWT key is void</param>
        /// <param name="claims">List of claims, such as roles</param>
        /// <returns>JWT Token</returns>
        /// <exception cref="Exception">If there are no audiences for the claim, throw an exception.</exception>
        private string TokenGenerator(string token, double expirationHours, List<Claim> claims)
        {
            var issuer = _configuration["Authentication:Schemes:Bearer:ValidIssuer"];
            var audiences = _configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<List<string>>();

            if (audiences == null)
                throw new Exception("There are no JWT audiences or failed to load configuration file");

            foreach(var item in audiences)
            {
                claims.Add(new Claim("aud", item));
            }

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken secToken = new JwtSecurityToken(
                issuer: issuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expirationHours),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(secToken);
        }

        public void DeleteRefreshTokens(int user_id)
        {
            throw new NotImplementedException();
        }

        public string CreateApiKey(int user_id)
        {
            throw new NotImplementedException();
        }

        public void DeleteApiKey(int user_id)
        {
            throw new NotImplementedException();
        }
    }
}
