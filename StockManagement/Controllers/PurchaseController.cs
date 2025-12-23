using Microsoft.AspNetCore.Mvc;
using StockManagement.Data;
using StockManagement.DTOs;
using StockManagement.Models;

[ApiController]
[Route("api/purchase")]
public class PurchaseController : ControllerBase
{
    private readonly AppDbContext _context;

    public PurchaseController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult PurchaseProduct(PurchaseDto dto)
    {
        // 1️⃣ Get Category
        var category = _context.Categories
            .FirstOrDefault(c => c.Name == dto.CategoryName);

        if (category == null)
            return BadRequest("Invalid category");

        // 2️⃣ Get Style
        var style = _context.Styles
            .FirstOrDefault(s => s.StyleName == dto.StyleName && s.CategoryId == category.CategoryId);

        if (style == null)
            return BadRequest("Invalid style");

        // 3️⃣ Get or Create Product
        var product = _context.Products
            .FirstOrDefault(p => p.ProductName == dto.ProductName && p.StyleId == style.StyleId);

        if (product == null)
        {
            product = new Product
            {
                StyleId = style.StyleId,
                ProductName = dto.ProductName,
                Price = dto.Price,
                ImageUrl = dto.ImageUrl
            };
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        // 4️⃣ Handle Multiple Sizes
        foreach (var sizeDto in dto.Sizes)
        {
            var size = _context.Sizes
                .FirstOrDefault(s => s.SizeName == sizeDto.SizeName);

            if (size == null)
                continue;

            var stock = _context.Stock.FirstOrDefault(
                s => s.ProductId == product.ProductId && s.SizeId == size.SizeId);

            if (stock == null)
            {
                stock = new Stock
                {
                    ProductId = product.ProductId,
                    SizeId = size.SizeId,
                    Quantity = sizeDto.Quantity
                };
                _context.Stock.Add(stock);
            }
            else
            {
                stock.Quantity += sizeDto.Quantity;
            }

            _context.Purchases.Add(new Purchase
            {
                ProductId = product.ProductId,
                SizeId = size.SizeId,
                Quantity = sizeDto.Quantity
            });
        }

        _context.SaveChanges();
        return Ok("Purchase successful with multiple sizes");
    }
}
