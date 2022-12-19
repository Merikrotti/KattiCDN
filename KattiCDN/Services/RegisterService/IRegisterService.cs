namespace KattiCDN.Services.RegisterService
{
    public interface IRegisterService
    {
        public Task<bool> RegisterUser(string username, string hashedpassword);
    }
}
