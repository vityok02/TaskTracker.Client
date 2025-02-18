namespace Domain.Models.Identity;

public class ChangePasswordModel
{
    public Guid UserId { get; set; }

    public string CurrentPassword { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;

    public string ConfirmedPassword { get; set; } = string.Empty;
}
