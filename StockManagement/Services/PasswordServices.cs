using Microsoft.AspNetCore.Identity;

public class PasswordService
{
    private readonly PasswordHasher<User> _hasher = new();

    public string Hash(string password, User user)
        => _hasher.HashPassword(user, password);

    public bool Verify(string hash, string password, User user)
        => _hasher.VerifyHashedPassword(user, hash, password)
           == PasswordVerificationResult.Success;
}
