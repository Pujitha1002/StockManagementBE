using StockManagement.Data;

using StockManagement.DTOs;

using StockManagement.Models;

using Microsoft.AspNetCore.Mvc;

namespace StockManagement.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class SaleController : ControllerBase

    {

        private readonly AppDbContext _context;

        public SaleController(AppDbContext context)

        {

            _context = context;

        }

        [HttpPost]

        public IActionResult CreateSale(SaleDto dto)

        {

            // 1️⃣ Check stock

            var stock = _context.Stock

                .FirstOrDefault(s => s.ProductId == dto.ProductId &&

                                     s.SizeName == dto.SizeName);

            if (stock == null)

                return BadRequest("Stock not found");

            if (stock.Quantity < dto.Quantity)

                return BadRequest("Insufficient stock");

            // 2️⃣ Reduce stock

            stock.Quantity -= dto.Quantity;

            // 3️⃣ Create sale record

            var sale = new Sale

            {

                ProductId = dto.ProductId,

                SizeName = dto.SizeName,

                Quantity = dto.Quantity,

                SaleDate = DateTime.Now

            };

            _context.Sales.Add(sale);

            _context.SaveChanges();

            return Ok("Sale completed successfully");

        }

    }

}

