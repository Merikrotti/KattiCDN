namespace KattiCDN.Services.PasswordService
{
    public class PasswordHasher : IPasswordHasher
    {
        public string GeneratePassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string hashedpassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedpassword);
        }
    }
}
