using KattiCDN.Models;
using KattiCDN.Requests;
using KattiCDN.Services;
using KattiCDN.Services.LoginService;
using KattiCDN.Services.PasswordService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace KattiCDN.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly string api_key = "hell0_friends";
        private readonly CdnDatabase _context;
        private readonly IPasswordHasher _pwhasher;
        private readonly ILoginDb _logindb;

        public ApiController(CdnDatabase context, IPasswordHasher pwhasher, ILoginDb logindb)
        {
            _context = context;
            _pwhasher = pwhasher;
            _logindb = logindb;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if(!ModelState.IsValid || request.Username == null || request.Password == null)
            {
                return BadRequest(new {error = "Input was not valid"});
            }

            var user = await _logindb.GetUserAsync(request.Username);

            if (user?.password == null)
                return Unauthorized();

            bool correctPassword = _pwhasher.VerifyPassword(request.Password, user.password);

            if (!correctPassword)
                return Unauthorized();
            
            return Ok( new {user=user});
        }

        [HttpPost("post")]
        public async Task<IActionResult> Post(UploadRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(new { error = "Input was not valid" });
            }

            if (request.api_key != api_key)
                return Unauthorized();

            string appPath = AppContext.BaseDirectory;

            string folderPath = Path.Combine(appPath, "images");

            if (request.file == null)
                return BadRequest(new {error = "File is broken (null) or server error has occurred"});

            string fileName = request.file.FileName;

            string filePath = Path.Combine(folderPath, fileName);



            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.file.CopyToAsync(fileStream);
            }

            return Ok(new { url = fileName});
        }
    }
}
