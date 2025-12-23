namespace StockManagement.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public int StyleId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
