namespace KattiCDN.Services.PasswordService
{
    public interface IPasswordHasher
    {
        string GeneratePassword(string password);
        bool VerifyPassword(string password, string hashedpassword);
    }
}
