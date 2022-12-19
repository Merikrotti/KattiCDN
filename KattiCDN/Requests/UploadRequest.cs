using System.ComponentModel.DataAnnotations;

namespace KattiCDN.Requests
{
    public class UploadRequest
    {
        [Required]
        public IFormFile? file { get; set; }
        [Required]
        public string? api_key { get; set; }
    }
}
