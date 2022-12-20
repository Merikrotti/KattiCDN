using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace KattiCDN.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string? username { get; set; }
        [Required]
        public string? password { get; set; }
        [Required]
        public string? confirmpassword { get; set; }

    }
}
