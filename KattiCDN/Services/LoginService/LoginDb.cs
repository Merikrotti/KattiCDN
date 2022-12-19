using KattiCDN.Models;
using Microsoft.EntityFrameworkCore;

namespace KattiCDN.Services.LoginService
{
    public class LoginDb : ILoginDb
    {
        private CdnDatabase _context;

        public LoginDb(CdnDatabase context)
        {
            _context = context;
        }

        /// <summary>
        /// Find user from database
        /// </summary>
        /// <param name="username">Username given by user</param>
        /// <param name="hashedpassword">Hashed password given by user</param>
        /// <returns>User or null</returns>
        public async Task<User?> LoginRequest(string username, string hashedpassword)
        {
            var user = await _context.users.Where(c => c.username == username && c.password == hashedpassword).FirstOrDefaultAsync();

            if (user == null) 
                return null;

            return user;
        }

        /// <summary>
        /// Find user with username from database
        /// </summary>
        /// <param name="username">Name of user</param>
        /// <returns>User with hashed password</returns>
        public async Task<User?> GetUserAsync(string username)
        {
            var user = await _context.users.Where(c => c.username == username).FirstOrDefaultAsync();
            if (user == null) return null;
            return user;
        }
    }
}
