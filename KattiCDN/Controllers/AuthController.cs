using KattiCDN.Requests;
using KattiCDN.Services.LoginService;
using KattiCDN.Services.RegisterService;
using KattiCDN.Services;
using Microsoft.AspNetCore.Mvc;
using KattiCDN.Services.PasswordService;
using Microsoft.AspNetCore.Authorization;
using KattiCDN.Services.TokenService;

namespace KattiCDN.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly CdnDatabase _context;
        private readonly IPasswordHasher _pwhasher;
        private readonly ILoginDb _logindb;
        private readonly IRegisterService _registerService;
        private readonly ITokenService _tokenService;

        public AuthController(CdnDatabase context, IPasswordHasher pwhasher, ILoginDb logindb, IRegisterService registerService, ITokenService tokenService)
        {
            _context = context;
            _pwhasher = pwhasher;
            _logindb = logindb;
            _registerService = registerService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            //Check if input is correct
            if (!ModelState.IsValid)
                return BadRequest();

            if (request.password == null || request.password.Length < 6)
                return BadRequest(new { error = "Password must be longer than 5 letters" });

            if (request.password != request.confirmpassword)
                return BadRequest(new { error = "Passwords do not match" });

            if (request.username == null || request.username.Length < 3)
                return BadRequest(new { error = "Username must be longer than 2 letters" });

            //Check if user already exists
            var user = await _logindb.GetUserAsync(request.username);

            if (user != null)
                return Conflict(new { error = "User already exists" });

            //Create user
            var hashedpassword = _pwhasher.GeneratePassword(request.password);

            var success = await _registerService.RegisterUser(request.username, hashedpassword);

            if (!success)
                return BadRequest(new { error = "Something went wrong registering..." });

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid || request.Username == null || request.Password == null)
            {
                return BadRequest(new { error = "Input was not valid" });
            }

            var user = await _logindb.GetUserAsync(request.Username);

            if (user?.password == null || user?.username == null)
                return Unauthorized();

            bool correctPassword = _pwhasher.VerifyPassword(request.Password, user.password);

            if (!correctPassword)
                return Unauthorized();

            var accesstoken = _tokenService.CreateAccessToken(user.username, user.id);

            return Ok(new { accessToken = accesstoken });
        }

        [Authorize]
        [HttpPost("authtest")]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
