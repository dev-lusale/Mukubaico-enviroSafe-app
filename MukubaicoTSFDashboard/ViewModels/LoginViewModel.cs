using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MukubaicoTSFDashboard.Services;

namespace MukubaicoTSFDashboard.ViewModels;

public class LoginViewModel : INotifyPropertyChanged
{
    private readonly AuthenticationService _authService;
    private string _username = "";
    private string _password = "";
    private string _fullName = "";
    private string _confirmPassword = "";
    private string _message = "";
    private bool _isLoading;
    private bool _isOperatorSelected;
    private bool _isResidentSelected;
    private bool _isLoginMode = true;

    public LoginViewModel(AuthenticationService authService)
    {
        _authService = authService;
        
        // Initialize commands
        SelectOperatorCommand = new Command(SelectOperator);
        SelectResidentCommand = new Command(SelectResident);
        SwitchToLoginCommand = new Command(SwitchToLogin);
        SwitchToRegisterCommand = new Command(SwitchToRegister);
        ActionCommand = new Command(async () => await ExecuteActionAsync(), () => !IsLoading);
    }

    // Properties
    public string Username
    {
        get => _username;
        set { _username = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public string FullName
    {
        get => _fullName;
        set { _fullName = value; OnPropertyChanged(); }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set { _confirmPassword = value; OnPropertyChanged(); }
    }

    public string Message
    {
        get => _message;
        set { _message = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasMessage)); }
    }

    public bool HasMessage => !string.IsNullOrEmpty(Message);

    public Color MessageColor => Message.Contains("success") || Message.Contains("created") ? Colors.Green : Colors.Red;

    public bool IsLoading
    {
        get => _isLoading;
        set 
        { 
            _isLoading = value; 
            OnPropertyChanged();
            ((Command)ActionCommand).ChangeCanExecute();
        }
    }

    public bool IsOperatorSelected
    {
        get => _isOperatorSelected;
        set 
        { 
            _isOperatorSelected = value; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(IsUserTypeSelected));
            UpdatePlaceholders();
        }
    }

    public bool IsResidentSelected
    {
        get => _isResidentSelected;
        set 
        { 
            _isResidentSelected = value; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(IsUserTypeSelected));
            UpdatePlaceholders();
        }
    }

    public bool IsUserTypeSelected => IsOperatorSelected || IsResidentSelected;

    public bool IsLoginMode
    {
        get => _isLoginMode;
        set 
        { 
            _isLoginMode = value; 
            OnPropertyChanged(); 
            OnPropertyChanged(nameof(IsRegisterMode));
            OnPropertyChanged(nameof(FormTitle));
            OnPropertyChanged(nameof(ActionButtonText));
            OnPropertyChanged(nameof(ActionButtonColor));
            ClearForm();
        }
    }

    public bool IsRegisterMode => !IsLoginMode;

    public string FormTitle => IsLoginMode ? 
        (IsOperatorSelected ? "Operator/Admin Sign In" : "Resident Sign In") :
        (IsOperatorSelected ? "Create Operator Account" : "Create Resident Account");

    public string ActionButtonText => IsLoginMode ? "SIGN IN" : "CREATE ACCOUNT";

    public Color ActionButtonColor => IsOperatorSelected ? Color.FromArgb("#1976D2") : Color.FromArgb("#4CAF50");

    public string UsernamePlaceholder => IsOperatorSelected ? "Enter username (e.g., username)" : "Enter username (e.g., username1)";

    public string PasswordPlaceholder => IsOperatorSelected ? "Enter password (e.g., test1234)" : "Enter password (e.g., test123)";

    // Commands
    public ICommand SelectOperatorCommand { get; }
    public ICommand SelectResidentCommand { get; }
    public ICommand SwitchToLoginCommand { get; }
    public ICommand SwitchToRegisterCommand { get; }
    public ICommand ActionCommand { get; }

    // Command implementations
    private void SelectOperator()
    {
        IsOperatorSelected = true;
        IsResidentSelected = false;
        ClearForm();
    }

    private void SelectResident()
    {
        IsOperatorSelected = false;
        IsResidentSelected = true;
        ClearForm();
    }

    private void SwitchToLogin()
    {
        IsLoginMode = true;
    }

    private void SwitchToRegister()
    {
        IsLoginMode = false;
    }

    private async Task ExecuteActionAsync()
    {
        if (IsLoginMode)
        {
            await LoginAsync();
        }
        else
        {
            await RegisterAsync();
        }
    }

    private async Task LoginAsync()
    {
        Message = "";
        
        if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
        {
            Message = "Please enter username and password";
            return;
        }

        IsLoading = true;

        var userType = IsOperatorSelected ? "Operator" : "Resident";
        var (success, message) = await _authService.LoginAsync(Username, Password, userType);

        IsLoading = false;

        if (success)
        {
            // Navigate to dashboard after successful login
            App.NavigateToDashboard();
        }
        else
        {
            Message = message;
        }
    }

    private async Task RegisterAsync()
    {
        Message = "";
        
        if (string.IsNullOrWhiteSpace(FullName) || string.IsNullOrWhiteSpace(Username) || 
            string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            Message = "Please fill in all fields";
            return;
        }

        if (Password != ConfirmPassword)
        {
            Message = "Passwords do not match";
            return;
        }

        if (Password.Length < 6)
        {
            Message = "Password must be at least 6 characters";
            return;
        }

        IsLoading = true;

        var userType = IsOperatorSelected ? "Operator" : "Resident";
        var (success, message) = await _authService.RegisterAsync(Username, Password, FullName, userType);

        IsLoading = false;

        if (success)
        {
            Message = "Account created successfully! You can now sign in.";
            await Task.Delay(2000);
            SwitchToLogin();
        }
        else
        {
            Message = message;
        }
    }

    private void UpdatePlaceholders()
    {
        OnPropertyChanged(nameof(UsernamePlaceholder));
        OnPropertyChanged(nameof(PasswordPlaceholder));
        OnPropertyChanged(nameof(FormTitle));
        OnPropertyChanged(nameof(ActionButtonColor));
    }

    private void ClearForm()
    {
        Username = "";
        Password = "";
        FullName = "";
        ConfirmPassword = "";
        Message = "";
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
