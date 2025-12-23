namespace StockManagement.Controllers
{
    public class LoginDto
    {
        public string EmailOrPhone { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}