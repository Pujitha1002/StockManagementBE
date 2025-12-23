using System;

namespace StockManagement.Models
{
    public class Sale
    {
        public int SaleId { get; set; }

        public int ProductId { get; set; }
        public int SizeId { get; set; }
        public int Quantity { get; set; }
        public DateTime SaleDate { get; set; } = DateTime.Now;
        public string SizeName { get; internal set; }
    }
}
