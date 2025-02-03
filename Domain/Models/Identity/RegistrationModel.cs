namespace Domain.Models.Identity;

public class RegisterModel
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string ConfirmedPassword { get; set; } = string.Empty;
}
