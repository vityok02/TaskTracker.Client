namespace Client;

public class ApplicationState
{
    private string _errorMessage = string.Empty;

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            ShowError();
        }
    }

    public event Action? OnNotification;

    public void ShowError()
    {
        OnNotification?.Invoke();
        _errorMessage = string.Empty;
    }
}