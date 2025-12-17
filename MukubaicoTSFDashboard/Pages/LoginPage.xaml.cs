using MukubaicoTSFDashboard.Services;
using MukubaicoTSFDashboard.ViewModels;

namespace MukubaicoTSFDashboard.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel(App.AuthService);
    }
}
