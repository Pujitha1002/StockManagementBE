namespace StockManagement.DTOs
{
    public class ProductCardDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool LowStock { get; set; }
        public List<SizeQuantityDto> Sizes { get; set; } = new();
    }
}
