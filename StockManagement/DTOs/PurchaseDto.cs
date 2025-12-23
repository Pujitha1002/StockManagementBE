namespace StockManagement.DTOs
{
    public class PurchaseDto
    {
        public string CategoryName { get; set; }
        public string StyleName { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public List<PurchaseSizeDto> Sizes { get; set; }
    }
}
