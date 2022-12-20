using KattiCDN.Models;
using KattiCDN.Requests;
using KattiCDN.Services;
using KattiCDN.Services.LoginService;
using KattiCDN.Services.PasswordService;
using KattiCDN.Services.RegisterService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace KattiCDN.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly string api_key = "hell0_friends";

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
