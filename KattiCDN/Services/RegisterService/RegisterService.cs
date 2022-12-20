using KattiCDN.Models;
using KattiCDN.Services.LoginService;

namespace KattiCDN.Services.RegisterService
{
    public class RegisterService : IRegisterService
    {
        private readonly CdnDatabase _context;

        public RegisterService(CdnDatabase context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates user into database
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="hashedpassword"></param>
        /// <returns>Returns true if succesful</returns>
        public async Task<bool> RegisterUser(string username, string hashedpassword)
        {
            var newUser = new User();
            newUser.username = username;
            newUser.password = hashedpassword;

            await _context.users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
