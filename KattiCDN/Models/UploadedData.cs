namespace KattiCDN.Models
{
    public class UploadedData
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string? filename { get; set; }
        public DateTime uploaddate { get; set; }
        public string? contenttype { get; set; }

    }
}
