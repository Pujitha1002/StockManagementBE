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
        // 1️⃣ Check if email already exists
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == dto.Email);

        // 2️⃣ Check if phone number already exists
        var phoneExists = await _context.Users
            .AnyAsync(u => u.PhoneNumber == dto.PhoneNumber);

        // 3️⃣ Return specific messages
        if (emailExists && phoneExists)
        {
            return Conflict(new
            {
                message = "Email and phone number are already registered"
            });
        }

        if (emailExists)
        {
            return Conflict(new
            {
                message = "Email is already registered"
            });
        }

        if (phoneExists)
        {
            return Conflict(new
            {
                message = "Phone number is already registered"
            });
        }


        // 2️⃣ Create user entity
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        // 3️⃣ Save to database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // 4️⃣ Success response
        return Ok(new
        {
            message = "Registered successfully"
        });
    }

    // ✅ SEND OTP
    [HttpPost("send-otp")]
    public async Task<IActionResult> SendOtp(SendOtpDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == dto.PhoneNumber);

        if (user == null)
            return BadRequest("Phone number not registered");

        var otpCode = new Random().Next(100000, 999999).ToString();

        var otp = new Otp
        {
            PhoneNumber = dto.PhoneNumber,
            Code = otpCode,
            ExpiryTime = DateTime.UtcNow.AddMinutes(5)
        };

        _context.Otps.Add(otp);
        await _context.SaveChangesAsync();

        // 🔔 SMS integration goes here (Twilio/Firebase)
        Console.WriteLine($"OTP: {otpCode}");

        return Ok("OTP sent successfully");
    }

    // ✅ VERIFY OTP
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
    {
        var otp = await _context.Otps
            .Where(o => o.PhoneNumber == dto.PhoneNumber && !o.IsUsed)
            .OrderByDescending(o => o.ExpiryTime)
            .FirstOrDefaultAsync();

        if (otp == null || otp.ExpiryTime < DateTime.UtcNow)
            return BadRequest("OTP expired");

        if (otp.Code != dto.Otp)
            return BadRequest("Invalid OTP");

        otp.IsUsed = true;
        await _context.SaveChangesAsync();

        return Ok("Login successful");
    }
}
