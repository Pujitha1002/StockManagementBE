namespace StockManagement.DTOs
{
    public class SaleDto
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public string SizeName { get; internal set; }
    }
}