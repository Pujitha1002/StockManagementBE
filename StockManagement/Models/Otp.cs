public class Otp
{
    public int OtpId { get; set; }

    public string PhoneNumber { get; set; } = null!;
    public string Code { get; set; } = null!;

    public DateTime ExpiryTime { get; set; }
    public bool IsUsed { get; set; } = false;
}
