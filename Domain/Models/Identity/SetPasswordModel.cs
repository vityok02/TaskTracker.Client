namespace Domain.Models.Identity;

public class SetPasswordModel
{
    public string ResetToken { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string ConfirmedPassword { get; set; } = string.Empty;
}
