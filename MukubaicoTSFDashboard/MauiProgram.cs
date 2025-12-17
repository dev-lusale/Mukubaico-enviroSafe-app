using Microsoft.Extensions.Logging;
using MukubaicoTSFDashboard.Services;
using Esri.ArcGISRuntime;
using Esri.ArcGISRuntime.Maui;
using Esri.ArcGISRuntime.Toolkit.Maui;
using Esri.ArcGISRuntime.Security;

namespace MukubaicoTSFDashboard;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseArcGISRuntime()
			.UseArcGISToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Initialize ArcGIS Runtime with authentication
		ArcGISRuntimeEnvironment.Initialize();
		
		// Configure ArcGIS authentication (basic setup)
		ConfigureBasicArcGISAuthentication();

		// Register services
		builder.Services.AddSingleton<QGISService>();
		builder.Services.AddSingleton<RealMapDataService>();
		// ArcGISAuthenticationService temporarily disabled due to API changes
		
		// Configure logging for QGIS service
		System.Diagnostics.Debug.WriteLine("🔧 Configuring Mukubaico TSF Dashboard services...");
		System.Diagnostics.Debug.WriteLine("📡 QGIS MapServer endpoint: http://localhost:8080/cgi-bin/qgis_mapserv.fcgi");

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	private static void ConfigureBasicArcGISAuthentication()
	{
		try
		{
			System.Diagnostics.Debug.WriteLine("🔐 Configuring basic ArcGIS authentication...");
			
			// Set up ArcGIS API Key for enhanced services
			// Note: This is optional - OpenStreetMap works without authentication
			var apiKey = GetArcGISApiKey();
			
			if (!string.IsNullOrEmpty(apiKey))
			{
				ArcGISRuntimeEnvironment.ApiKey = apiKey;
				System.Diagnostics.Debug.WriteLine("✅ ArcGIS API Key configured successfully");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("ℹ️ No ArcGIS API Key - using free basemaps only");
			}
			
			System.Diagnostics.Debug.WriteLine("✅ Basic ArcGIS authentication configured");
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"⚠️ ArcGIS authentication setup warning: {ex.Message}");
		}
	}
	
	private static string? GetArcGISApiKey()
	{
		// Priority order for API key configuration:
		// 1. Environment variable (for production)
		// 2. Configuration file (for development)
		// 3. Hardcoded key (for testing - not recommended for production)
		
		// Check environment variable first (recommended for production)
		var envKey = Environment.GetEnvironmentVariable("ARCGIS_API_KEY");
		if (!string.IsNullOrEmpty(envKey))
		{
			System.Diagnostics.Debug.WriteLine("🔑 Using ArcGIS API Key from environment variable");
			return envKey;
		}
		
		// For development/testing - replace with your actual API key
		// Get your free API key from: https://developers.arcgis.com/api-keys/
		var developmentKey = "YOUR_ARCGIS_API_KEY_HERE";
		
		if (developmentKey != "YOUR_ARCGIS_API_KEY_HERE")
		{
			System.Diagnostics.Debug.WriteLine("🔑 Using development ArcGIS API Key");
			return developmentKey;
		}
		
		System.Diagnostics.Debug.WriteLine("ℹ️ No ArcGIS API Key configured - using free services only");
		return null;
	}
}
