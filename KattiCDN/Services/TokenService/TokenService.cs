using Microsoft.EntityFrameworkCore;
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

        /// <summary>
        /// Create an access token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="user_id"></param>
        /// <returns>Access token</returns>
        public string CreateAccessToken(string username, int user_id)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, username));
            claims.Add(new Claim("uid", user_id.ToString()));

            var accesstoken = TokenGenerator(0.1, claims);

            return accesstoken;
        }

        /// <summary>
        /// Create a refresh token for user and link user_id to it
        /// </summary>
        /// <param name="user_id">Logged in user</param>
        /// <returns>Refresh token</returns>
        public async Task<string> CreateRefreshToken(int user_id)
        {
            var refreshToken = TokenGenerator(336.0, new List<Claim>());

            await _context.refreshtokens.AddAsync(new Models.RefreshToken() { user_id = user_id, token = refreshToken});
            await _context.SaveChangesAsync();

            return refreshToken;
        }

        /// <summary>
        /// Generates JWT key to use
        /// </summary>
        /// <param name="expirationHours">How many hours until the JWT key is void</param>
        /// <param name="claims">List of claims, such as roles</param>
        /// <returns>JWT Token</returns>
        /// <exception cref="Exception">If there are no audiences for the claim, throw an exception.</exception>
        /// <exception cref="ArgumentNullException">Either appsettings.json cannot be read or you have not given a token</exception>
        private string TokenGenerator(double expirationHours, List<Claim> claims)
        {
            var token = _configuration["Authentication:Schemes:Bearer:Token"] ?? throw new ArgumentNullException("Configuration token should not be null.");
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

        /// <summary>
        /// Delete all refresh tokens created by user (logout)
        /// </summary>
        /// <param name="user_id">Logged in user</param>
        public async Task DeleteRefreshTokens(int user_id)
        {
            var selectedtokens = await _context.refreshtokens.Where(c => c.user_id == user_id).ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Validates (queries) the refresh token, used to generate a new refreshtoken and an accesstoken
        /// </summary>
        /// <param name="refreshToken">Given refreshtoken</param>
        /// <returns>-1 if not found, otherwise user id</returns>
        public async Task<int> ValidateRefreshToken(string refreshToken)
        {
            var query = await _context.refreshtokens.Where(c => c.token == refreshToken).FirstOrDefaultAsync();
            if (query == null) return -1;
            return query.user_id;
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
