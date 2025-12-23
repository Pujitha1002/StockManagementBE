using System;

namespace StockManagement.Models
{
    public class Purchase
    {
        public int PurchaseId { get; set; }
        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}
