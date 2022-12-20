using KattiCDN.Models;

namespace KattiCDN.Services.LoginService
{
    public interface ILoginDb
    {
        public Task<User?> LoginRequest(string username, string hashedpassword);
        public Task<User?> GetUserAsync(string username);
        public Task<User?> GetUserByIdAsync(int user_id);
    }
}
