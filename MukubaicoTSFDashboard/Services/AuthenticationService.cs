using MukubaicoTSFDashboard.Models;

namespace MukubaicoTSFDashboard.Services;

public class AuthenticationService
{
    private readonly List<User> _users = new()
    {
        // Default Operator/Admin accounts
        new User { Username = "username", Password = "test1234", FullName = "System Administrator", Role = "Admin", UserType = "Operator" },
        new User { Username = "admin", Password = "admin123", FullName = "Administrator", Role = "Admin", UserType = "Operator" },
        new User { Username = "operator", Password = "operator123", FullName = "System Operator", Role = "Operator", UserType = "Operator" },
        
        // Default Resident accounts
        new User { Username = "username1", Password = "test123", FullName = "Community Resident", Role = "Resident", UserType = "Resident" },
        new User { Username = "resident", Password = "resident123", FullName = "Local Resident", Role = "Resident", UserType = "Resident" },
        
        // Legacy viewer account
        new User { Username = "viewer", Password = "viewer123", FullName = "Viewer", Role = "Viewer", UserType = "Operator" }
    };

    public User? CurrentUser { get; private set; }

    public async Task<(bool Success, string Message)> LoginAsync(string username, string password, string userType = "")
    {
        await Task.Delay(500); // Simulate network delay

        var user = _users.FirstOrDefault(u => 
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
            u.Password == password);

        if (user != null)
        {
            // If userType is specified, verify it matches
            if (!string.IsNullOrEmpty(userType) && user.UserType != userType)
            {
                return (false, $"This account is not registered as a {userType}");
            }

            user.LastLogin = DateTime.Now;
            CurrentUser = user;
            return (true, "Login successful");
        }

        return (false, "Invalid username or password");
    }

    public async Task<(bool Success, string Message)> RegisterAsync(string username, string password, string fullName, string userType)
    {
        await Task.Delay(500); // Simulate network delay

        // Check if username already exists
        if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
        {
            return (false, "Username already exists");
        }

        // Validate username format
        if (username.Length < 3)
        {
            return (false, "Username must be at least 3 characters");
        }

        // Create new user
        var role = userType == "Operator" ? "Operator" : "Resident";
        var newUser = new User
        {
            Username = username,
            Password = password,
            FullName = fullName,
            Role = role,
            UserType = userType,
            CreatedDate = DateTime.Now
        };

        _users.Add(newUser);
        return (true, "Account created successfully");
    }

    public void Logout()
    {
        CurrentUser = null;
    }

    public bool IsAuthenticated => CurrentUser != null;

    public List<User> GetUsersByType(string userType)
    {
        return _users.Where(u => u.UserType == userType).ToList();
    }

    public int GetUserCount(string userType)
    {
        return _users.Count(u => u.UserType == userType);
    }
}
