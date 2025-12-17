using MukubaicoTSFDashboard.Services;
using MukubaicoTSFDashboard.Pages;

namespace MukubaicoTSFDashboard;

public partial class App : Application
{
	public static AuthenticationService AuthService { get; private set; } = new();
	public static QGISService QGISService { get; private set; } = new();

	public App()
	{
		InitializeComponent();
		
		// Initialize QGIS connection at startup
		InitializeQGISConnectionAsync();
	}

	private async void InitializeQGISConnectionAsync()
	{
		try
		{
			System.Diagnostics.Debug.WriteLine("🚀 Starting QGIS MapServer connection at startup...");
			
#if WINDOWS
			// Attempt to connect to QGIS MapServer
			var connected = await QGISService.ConnectToQGISServerAsync("http://localhost:8080/cgi-bin/qgis_mapserv.fcgi");
			
			if (connected)
			{
				System.Diagnostics.Debug.WriteLine("✅ QGIS MapServer connected successfully at startup");
				
				// Initialize with real data
				await QGISService.InitializeWithRealDataAsync();
				System.Diagnostics.Debug.WriteLine("📊 QGIS service initialized with real TSF data");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("⚠️ QGIS MapServer connection failed - running in simulation mode");
				System.Diagnostics.Debug.WriteLine("💡 To connect to QGIS MapServer:");
				System.Diagnostics.Debug.WriteLine("   1. Ensure QGIS Server is running on localhost:8080");
				System.Diagnostics.Debug.WriteLine("   2. Verify CGI endpoint: /cgi-bin/qgis_mapserv.fcgi");
				System.Diagnostics.Debug.WriteLine("   3. Check QGIS project has 'tsf_facilities' layer");
			}
#else
			System.Diagnostics.Debug.WriteLine("📱 Platform: Running in simulation mode (non-Windows)");
			await QGISService.InitializeWithRealDataAsync();
#endif
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"❌ QGIS startup connection error: {ex.Message}");
			System.Diagnostics.Debug.WriteLine("🔄 Falling back to simulation mode");
		}
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Always start with login page
		return new Window(new NavigationPage(new LoginPage()));
	}

	public static void NavigateToDashboard()
	{
		// Navigate to the main dashboard after successful login
		if (Application.Current?.Windows.Count > 0)
		{
			Application.Current.Windows[0].Page = new AppShell();
		}
	}
}