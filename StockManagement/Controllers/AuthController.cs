using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;

    public AuthController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _context.User.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email
        };

        user.PasswordHash = _password.Hash(dto.Password, user);

        _context.User.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Registered successfully");
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.User
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user == null)
            return Unauthorized("Invalid email or password");

        var valid = _password.Verify(
            user.PasswordHash,
            dto.Password,
            user
        );

        if (!valid)
            return Unauthorized("Invalid email or password");

        var token = _jwt.GenerateToken(user);

        return Ok(new
        {
            token = token,
            user = new
            {
                user.Id,
                user.FirstName,
                user.Email
            }
        });
    }

