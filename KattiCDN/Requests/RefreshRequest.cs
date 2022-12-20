using System.ComponentModel.DataAnnotations;

namespace KattiCDN.Requests
{
    public class RefreshRequest
    {
        [Required]
        public string? refreshToken { get; set; }
    }
}
