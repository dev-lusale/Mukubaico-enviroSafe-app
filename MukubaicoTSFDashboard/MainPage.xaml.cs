using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime;
using MukubaicoTSFDashboard.Services;
using MukubaicoTSFDashboard.Models;
using System.Diagnostics;
using EsriMap = Esri.ArcGISRuntime.Mapping.Map;

namespace MukubaicoTSFDashboard;

public partial class MainPage : ContentPage
{
    private readonly RealMapDataService _realMapService;
    private readonly GraphicsOverlay _tsfOverlay = new();
    private readonly GraphicsOverlay _monitoringOverlay = new();
    private bool _isMapInitialized = false;
    private List<TSFLocation> _tsfLocations = [];

    public MainPage()
    {
        InitializeComponent();
        _realMapService = new RealMapDataService();
        
        // Initialize the dashboard map
        _ = InitializeDashboardMapAsync();
    }

    private async Task InitializeDashboardMapAsync()
    {
        try
        {
            // Load real TSF data
            _tsfLocations = _realMapService.GetRealTSFLocations();
            
            // Create map with OpenStreetMap basemap (no authentication required)
            var map = new EsriMap(BasemapStyle.OpenOSMStyle);
            DashboardMapView.Map = map;
            
            // Add graphics overlays
            DashboardMapView.GraphicsOverlays?.Add(_tsfOverlay);
            DashboardMapView.GraphicsOverlays?.Add(_monitoringOverlay);
            
            // Wait for map to load
            await map.LoadAsync();
            
            if (map.LoadStatus == LoadStatus.Loaded)
            {
                // Add TSF locations to map
                await AddDashboardTSFLocationsAsync();
                
                // Set initial viewpoint to Zambia Copperbelt region
                var copperBeltCenter = new MapPoint(27.8, -12.5, SpatialReferences.Wgs84);
                await DashboardMapView.SetViewpointCenterAsync(copperBeltCenter, 300000); // 300km scale for overview
                
                // Wire up map events
                DashboardMapView.GeoViewTapped += OnDashboardMapTapped;
                
                // Hide loading indicator
                DashboardLoadingFrame.IsVisible = false;
                DashboardLoadingIndicator.IsRunning = false;
                _isMapInitialized = true;
            }
            else
            {
                Debug.WriteLine($"Dashboard map failed to load. Status: {map.LoadStatus}");
                DashboardLoadingFrame.IsVisible = false;
                DashboardLoadingIndicator.IsRunning = false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Dashboard map initialization error: {ex}");
            DashboardLoadingFrame.IsVisible = false;
            DashboardLoadingIndicator.IsRunning = false;
        }
    }

    private async Task AddDashboardTSFLocationsAsync()
    {
        await Task.Delay(1); // Ensure UI thread
        
        foreach (var tsf in _tsfLocations)
        {
            // Create point geometry
            var point = new MapPoint(tsf.Longitude, tsf.Latitude, SpatialReferences.Wgs84);
            
            // Create symbol based on risk level
            SimpleMarkerSymbol symbol = tsf.RiskLevel.ToLower() switch
            {
                "high" => new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.FromArgb(255, 87, 34), 12),
                "medium" => new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.FromArgb(255, 152, 0), 10),
                "low" => new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.FromArgb(76, 175, 80), 8),
                _ => new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.Gray, 8)
            };
            
            // Create graphic
            var graphic = new Graphic(point, symbol);
            graphic.Attributes["Name"] = tsf.Name;
            graphic.Attributes["RiskLevel"] = tsf.RiskLevel;
            graphic.Attributes["Status"] = tsf.Status;
            graphic.Attributes["Capacity"] = tsf.Capacity;
            
            _tsfOverlay.Graphics.Add(graphic);
        }
        
        // Add monitoring stations as smaller blue dots
        var monitoringStations = _realMapService.GetRealMonitoringStations();
        foreach (var station in monitoringStations)
        {
            var point = new MapPoint(station.Longitude, station.Latitude, SpatialReferences.Wgs84);
            var symbol = new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, System.Drawing.Color.FromArgb(33, 150, 243), 6);
            
            var graphic = new Graphic(point, symbol);
            graphic.Attributes["Name"] = station.Name;
            graphic.Attributes["Type"] = "Monitoring Station";
            
            _monitoringOverlay.Graphics.Add(graphic);
        }
    }

    private async void OnDashboardMapTapped(object? sender, Esri.ArcGISRuntime.Maui.GeoViewInputEventArgs e)
    {
        try
        {
            if (!_isMapInitialized) return;

            // Identify graphics at the tapped location
            var identifyResults = await DashboardMapView.IdentifyGraphicsOverlaysAsync(e.Position, 10, false);
            
            if (identifyResults.Any())
            {
                var graphic = identifyResults.First().Graphics.First();
                var name = graphic.Attributes["Name"]?.ToString() ?? "Unknown";
                var type = graphic.Attributes.ContainsKey("Type") ? graphic.Attributes["Type"]?.ToString() : "TSF Facility";
                
                if (type == "TSF Facility")
                {
                    var riskLevel = graphic.Attributes["RiskLevel"]?.ToString() ?? "Unknown";
                    var capacity = graphic.Attributes["Capacity"]?.ToString() ?? "Unknown";
                    
                    await DisplayAlert("TSF Facility Details", 
                        $"📍 {name}\n\n" +
                        $"⚠️ Risk Level: {riskLevel}\n" +
                        $"📊 Capacity: {capacity:N0} m³\n" +
                        $"🗺️ Location: Zambia Copperbelt\n" +
                        $"🏭 Mining Operation", "OK");
                }
                else
                {
                    await DisplayAlert("Monitoring Station", 
                        $"📡 {name}\n\n" +
                        $"📊 Environmental monitoring station\n" +
                        $"🔄 Real-time data collection\n" +
                        $"🗺️ Location: Zambia Copperbelt", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Dashboard map tap error: {ex}");
        }
    }

    private async void OnDashboardZoomInClicked(object? sender, EventArgs e)
    {
        if (!_isMapInitialized) return;
        
        try
        {
            var currentViewpoint = DashboardMapView.GetCurrentViewpoint(ViewpointType.CenterAndScale);
            if (currentViewpoint != null)
            {
                var newScale = currentViewpoint.TargetScale * 0.5; // Zoom in by 50%
                await DashboardMapView.SetViewpointScaleAsync(newScale);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Dashboard zoom in error: {ex}");
        }
    }

    private async void OnDashboardZoomOutClicked(object? sender, EventArgs e)
    {
        if (!_isMapInitialized) return;
        
        try
        {
            var currentViewpoint = DashboardMapView.GetCurrentViewpoint(ViewpointType.CenterAndScale);
            if (currentViewpoint != null)
            {
                var newScale = currentViewpoint.TargetScale * 2.0; // Zoom out by 100%
                await DashboardMapView.SetViewpointScaleAsync(newScale);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Dashboard zoom out error: {ex}");
        }
    }

    private async void OnDashboardCenterClicked(object? sender, EventArgs e)
    {
        if (!_isMapInitialized) return;
        
        try
        {
            // Center on Zambia Copperbelt region
            var copperBeltCenter = new MapPoint(27.8, -12.5, SpatialReferences.Wgs84);
            await DashboardMapView.SetViewpointCenterAsync(copperBeltCenter, 300000);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Dashboard center error: {ex}");
        }
    }
}
