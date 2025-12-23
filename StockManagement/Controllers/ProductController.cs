using Microsoft.AspNetCore.Mvc;
using StockManagement.Data;
using StockManagement.DTOs;

namespace StockManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // 1️⃣ Women / Men / Kids count
        [HttpGet("count/category/{categoryId}")]
        public IActionResult GetProductCountByCategory(int categoryId)
        {
            var count = _context.Products
                .Join(_context.Styles,
                      p => p.StyleId,
                      s => s.StyleId,
                      (p, s) => new { p, s })
                .Count(x => x.s.CategoryId == categoryId);

            return Ok(count);
        }
        [HttpGet("count/style/{styleId}")]
        public IActionResult GetProductCountByStyle(int styleId)
        {
            var count = _context.Products.Count(p => p.StyleId == styleId);
            return Ok(count);
        }

        [HttpGet("style/{styleId}")]
        public IActionResult GetProductsByStyle(int styleId)
        {
            var products = _context.Products
                .Join(_context.Styles,
                      p => p.StyleId,
                      s => s.StyleId,
                      (p, s) => new { p, s })
                .Where(x => x.p.StyleId == styleId
                         && x.s.CategoryId == 1) // ✅ 1 = Women
                .Select(x => new ProductCardDto
                {
                    ProductId = x.p.ProductId,
                    ProductName = x.p.ProductName,
                    Price = x.p.Price,
                    ImageUrl = x.p.ImageUrl,

                    Sizes = _context.Stock
                        .Where(st => st.ProductId == x.p.ProductId)
                        .Join(_context.Sizes,
                              st => st.SizeId,
                              sz => sz.SizeId,
                              (st, sz) => new SizeQuantityDto
                              {
                                  Size = sz.SizeName,
                                  Quantity = st.Quantity
                              })
                        .ToList()
                })
                .ToList();

            // Low stock calculation (UNCHANGED)
            foreach (var product in products)
            {
                product.LowStock = product.Sizes.Any(s => s.Quantity < 3);
            }

            return Ok(products);
        }


    }
}

