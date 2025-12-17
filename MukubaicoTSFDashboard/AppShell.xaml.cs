using MukubaicoTSFDashboard.Pages;

namespace MukubaicoTSFDashboard;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Register routes for navigation
		Routing.RegisterRoute("SimpleArcGISMapPage", typeof(SimpleArcGISMapPage));
		Routing.RegisterRoute("QGISMapPage", typeof(QGISMapPage));
	}

	private void OnLogoutClicked(object sender, EventArgs e)
	{
		App.AuthService.Logout();
		
		// Return to login page
		if (Application.Current?.Windows.Count > 0)
		{
			Application.Current.Windows[0].Page = new NavigationPage(new LoginPage());
		}
	}
}
