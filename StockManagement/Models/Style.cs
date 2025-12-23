namespace StockManagement.Models
{
    public class Style
    {
        public int StyleId { get; set; }
        public int CategoryId { get; set; }
        public string StyleName { get; set; } = string.Empty;
    }
}
