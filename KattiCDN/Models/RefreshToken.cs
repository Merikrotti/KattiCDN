namespace KattiCDN.Models
{
    public class RefreshToken
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string? token { get; set; }
    }
}
