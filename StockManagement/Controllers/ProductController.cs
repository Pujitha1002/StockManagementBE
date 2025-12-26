using Microsoft.AspNetCore.Mvc;
using StockManagement.Data;
using StockManagement.DTOs;
using StockManagement.Models;

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

        [HttpGet("styles/category/{categoryId}")]
        public IActionResult GetStylesByCategory(int categoryId)
        {
            var styles = _context.Styles
                .Where(s => s.CategoryId == categoryId)
                .Select(s => new
                {
                    styleId = s.StyleId,
                    name = s.StyleName
                })
                .OrderBy(s => s.name)
                .ToList();

            return Ok(styles);
        }

        [HttpPost("styles")]
        public IActionResult AddStyle(AddStyleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Style name is required");

            // 🔹 prevent duplicates within same category
            bool exists = _context.Styles.Any(s =>
                s.CategoryId == dto.CategoryId &&
                s.StyleName.ToLower() == dto.Name.ToLower()
            );

            if (exists)
                return Conflict("Style already exists");

            var style = new Style
            {
                StyleName = dto.Name,
                CategoryId = dto.CategoryId
            };

            _context.Styles.Add(style);
            _context.SaveChanges();

            return Ok(new
            {
                styleId = style.StyleId,
                name = style.StyleName
            });
        }
        [HttpGet("category/{categoryId}")]
        public IActionResult GetProductsByCategory(int categoryId)
        {
            var products = _context.Products
                .Join(_context.Styles,
                      p => p.StyleId,
                      s => s.StyleId,
                      (p, s) => new { p, s })
                .Where(x => x.s.CategoryId == categoryId)
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

            foreach (var product in products)
                product.LowStock = product.Sizes.Any(s => s.Quantity < 3);

            return Ok(new
            {
                count = products.Count,
                products
            });
        }


        [HttpPut("{productId}/price")]
        public IActionResult UpdateProductPrice(int productId, [FromBody] decimal newPrice)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == productId);

            if (product == null)
                return NotFound("Product not found");

            if (newPrice <= 0)
                return BadRequest("Invalid price");

            product.Price = newPrice;
            _context.SaveChanges();

            return Ok(product.Price);
        }


        [HttpGet("style/{styleId}")]
        public IActionResult GetProductsByStyle(int styleId)
        {
            var products = _context.Products
                .Where(p => p.StyleId == styleId)
                .Select(p => new ProductCardDto
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,

                    Sizes = _context.Stock
                        .Where(st => st.ProductId == p.ProductId)
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

            foreach (var product in products)
            {
                product.LowStock = product.Sizes.Any(s => s.Quantity < 3);
            }

            return Ok(new
            {
                count = products.Count,
                products
            });
        }



    }
}

