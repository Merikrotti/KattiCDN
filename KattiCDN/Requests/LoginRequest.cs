using System.ComponentModel.DataAnnotations;

namespace KattiCDN.Requests
{
    public class LoginRequest
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
