namespace MukubaicoTSFDashboard.Models;

public class User
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string FullName { get; set; } = "";
    public string Role { get; set; } = "";
    public string UserType { get; set; } = ""; // "Operator" or "Resident"
    public DateTime LastLogin { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
