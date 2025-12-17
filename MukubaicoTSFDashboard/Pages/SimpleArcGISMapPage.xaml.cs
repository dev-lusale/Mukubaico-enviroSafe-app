using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Mapping;
using Esri.ArcGISRuntime.Symbology;
using Esri.ArcGISRuntime.UI;
using Esri.ArcGISRuntime;
using System.Diagnostics;
using EsriMap = Esri.ArcGISRuntime.Mapping.Map;

namespace MukubaicoTSFDashboard.Pages;

public partial class SimpleArcGISMapPage : ContentPage
{
    private bool _isMapLoaded = false;
    private readonly GraphicsOverlay _tsfOverlay = new();

    public SimpleArcGISMapPage()
    {
        InitializeComponent();
        _ = InitializeMapAsync();
    }

    private async Task InitializeMapAsync()
    {
        try
        {
            LoadingStatusLabel.Text = "Initializing ArcGIS Map...";
            StatusLabel.Text = "Starting initialization...";
            
            // Create map with OpenStreetMap basemap (no authentication required)
            var map = new EsriMap(BasemapStyle.OpenOSMStyle);
            MainMapView.Map = map;
            
            LoadingStatusLabel.Text = "Loading map...";
            StatusLabel.Text = "Loading basemap tiles...";
            
            // Wait for map to load
            await map.LoadAsync();
            
            LoadingStatusLabel.Text = "Waiting for map to load...";
            
            if (map.LoadStatus == LoadStatus.Loaded)
            {
                LoadingStatusLabel.Text = "Map loaded successfully! Adding TSF data...";
                StatusLabel.Text = "Map loaded - Adding data...";
                
                // Add graphics overlay for TSF locations
                MainMapView.GraphicsOverlays?.Add(_tsfOverlay);
                
                // Add TSF locations
                await AddTSFLocationsAsync();
                
                // Set initial viewpoint to Zambia Copperbelt region
                var zambiaCenter = new MapPoint(27.8, -12.5, SpatialReferences.Wgs84);
                await MainMapView.SetViewpointCenterAsync(zambiaCenter, 500000);
                
                // Wire up map events
                MainMapView.GeoViewTapped += OnMapTapped;
                MainMapView.ViewpointChanged += OnViewpointChanged;
                
                // Hide loading indicator
                LoadingFrame.IsVisible = false;
                _isMapLoaded = true;
                
                StatusLabel.Text = "Ready - Map showing Zambia with TSF locations";
                CoordinatesLabel.Text = "Coordinates: -12.5°, 27.8° (Zambia Copperbelt)";
                
                await DisplayAlert("Success!", 
                    "ArcGIS Map is now operational with TSF data!\n\n" +
                    "• 5 real TSF facilities in Zambia's Copperbelt\n" +
                    "• Interactive map with zoom and pan\n" +
                    "• Tap on facilities for details\n" +
                    "• Color-coded risk levels", "OK");
            }
            else
            {
                await DisplayAlert("Error", $"Map failed to load. Status: {map.LoadStatus}", "OK");
                StatusLabel.Text = $"Error: Map status {map.LoadStatus}";
                LoadingFrame.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to initialize map: {ex.Message}", "OK");
            StatusLabel.Text = $"Error: {ex.Message}";
            LoadingFrame.IsVisible = false;
            Debug.WriteLine($"Map initialization error: {ex}");
        }
    }

    private async Task AddTSFLocationsAsync()
    {
        await Task.Delay(1); // Ensure UI thread
        
        // Real TSF locations in Zambia's Copperbelt
        var tsfLocations = new[]
        {
            new { Name = "Konkola Copper Mines TSF", Lat = -12.4964, Lon = 27.6256, Risk = "Medium", Owner = "Vedanta Resources" },
            new { Name = "Nchanga Copper Mine TSF", Lat = -12.1328, Lon = 27.4467, Risk = "Low", Owner = "Konkola Copper Mines" },
            new { Name = "Mufulira Mine TSF", Lat = -12.5497, Lon = 28.2409, Risk = "High", Owner = "Mopani Copper Mines" },
            new { Name = "Kitwe Copper TSF", Lat = -12.8024, Lon = 28.2132, Risk = "Low", Owner = "Mopani Copper Mines" },
            new { Name = "Chingola Mine TSF", Lat = -12.5289, Lon = 27.8642, Risk = "Medium", Owner = "Konkola Copper Mines" }
        };

        foreach (var tsf in tsfLocations)
        {
            try
            {
                var point = new MapPoint(tsf.Lon, tsf.Lat, SpatialReferences.Wgs84);
                
                // Create symbol based on risk level
                var symbol = CreateTSFSymbol(tsf.Risk);
                
                var graphic = new Graphic(point, symbol);
                graphic.Attributes["Name"] = tsf.Name;
                graphic.Attributes["RiskLevel"] = tsf.Risk;
                graphic.Attributes["Owner"] = tsf.Owner;
                graphic.Attributes["Latitude"] = tsf.Lat;
                graphic.Attributes["Longitude"] = tsf.Lon;
                
                _tsfOverlay.Graphics.Add(graphic);
                
                // Add label
                var textSymbol = new TextSymbol(tsf.Name, System.Drawing.Color.DarkBlue, 10, 
                    Esri.ArcGISRuntime.Symbology.HorizontalAlignment.Center, 
                    Esri.ArcGISRuntime.Symbology.VerticalAlignment.Bottom);
                textSymbol.OffsetY = 15;
                textSymbol.HaloColor = System.Drawing.Color.White;
                textSymbol.HaloWidth = 2;
                
                var labelGraphic = new Graphic(point, textSymbol);
                _tsfOverlay.Graphics.Add(labelGraphic);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding TSF {tsf.Name}: {ex.Message}");
            }
        }
    }

    private static SimpleMarkerSymbol CreateTSFSymbol(string riskLevel)
    {
        var color = riskLevel.ToLower() switch
        {
            "low" => System.Drawing.Color.Green,
            "medium" => System.Drawing.Color.Orange,
            "high" => System.Drawing.Color.Red,
            _ => System.Drawing.Color.Gray
        };

        return new SimpleMarkerSymbol(SimpleMarkerSymbolStyle.Circle, color, 12)
        {
            Outline = new SimpleLineSymbol(SimpleLineSymbolStyle.Solid, System.Drawing.Color.White, 2)
        };
    }

    private void OnMapTapped(object? sender, Esri.ArcGISRuntime.Maui.GeoViewInputEventArgs e)
    {
        try
        {
            if (!_isMapLoaded) return;

            var screenPoint = new Point(e.Position.X, e.Position.Y);
            _ = Task.Run(async () =>
            {
                var identifyResults = await MainMapView.IdentifyGraphicsOverlaysAsync(screenPoint, 10, false);
                
                foreach (var result in identifyResults)
                {
                    if (result.Graphics.Count > 0)
                    {
                        var graphic = result.Graphics[0];
                        
                        if (graphic.Attributes.ContainsKey("Name"))
                        {
                            await MainThread.InvokeOnMainThreadAsync(async () =>
                            {
                                var name = graphic.Attributes["Name"]?.ToString() ?? "Unknown";
                                var risk = graphic.Attributes["RiskLevel"]?.ToString() ?? "Unknown";
                                var owner = graphic.Attributes["Owner"]?.ToString() ?? "Unknown";
                                var lat = graphic.Attributes["Latitude"]?.ToString() ?? "Unknown";
                                var lon = graphic.Attributes["Longitude"]?.ToString() ?? "Unknown";
                                
                                await DisplayAlert("TSF Facility Details", 
                                    $"Facility: {name}\n" +
                                    $"Risk Level: {risk}\n" +
                                    $"Owner: {owner}\n" +
                                    $"Location: {lat}°, {lon}°\n" +
                                    $"Status: Active Monitoring", "OK");
                            });
                            return;
                        }
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Map tap error: {ex.Message}");
        }
    }

    private void OnViewpointChanged(object? sender, EventArgs e)
    {
        try
        {
            if (_isMapLoaded)
            {
                var viewpoint = MainMapView.GetCurrentViewpoint(ViewpointType.CenterAndScale);
                if (viewpoint?.TargetGeometry is MapPoint center)
                {
                    CoordinatesLabel.Text = $"Coordinates: {center.Y:F4}°, {center.X:F4}°";
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Viewpoint change error: {ex.Message}");
        }
    }

    private async void OnRefreshClicked(object? sender, EventArgs e)
    {
        try
        {
            RefreshButton.IsEnabled = false;
            RefreshButton.Text = "🔄 Refreshing...";
            
            if (_isMapLoaded)
            {
                // Refresh TSF data
                _tsfOverlay.Graphics.Clear();
                await AddTSFLocationsAsync();
                
                await DisplayAlert("Success", "TSF data refreshed successfully!", "OK");
            }
            else
            {
                await DisplayAlert("Info", "Map is still loading. Please wait.", "OK");
            }
            
            RefreshButton.Text = "🔄 Refresh";
            RefreshButton.IsEnabled = true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Refresh failed: {ex.Message}", "OK");
            RefreshButton.Text = "🔄 Refresh";
            RefreshButton.IsEnabled = true;
        }
    }

    private async void OnZoomToZambiaClicked(object? sender, EventArgs e)
    {
        try
        {
            if (_isMapLoaded)
            {
                var zambiaCenter = new MapPoint(27.8, -12.5, SpatialReferences.Wgs84);
                await MainMapView.SetViewpointCenterAsync(zambiaCenter, 500000);
                StatusLabel.Text = "Zoomed to Zambia Copperbelt region";
            }
            else
            {
                await DisplayAlert("Info", "Map is still loading. Please wait.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Zoom failed: {ex.Message}", "OK");
        }
    }

    private async void OnShowTSFClicked(object? sender, EventArgs e)
    {
        try
        {
            if (_isMapLoaded)
            {
                var tsfCount = _tsfOverlay.Graphics.Count / 2; // Divided by 2 because we have symbols + labels
                await DisplayAlert("TSF Facilities", 
                    $"Currently showing {tsfCount} TSF facilities in Zambia's Copperbelt:\n\n" +
                    "🔴 High Risk: Mufulira Mine TSF\n" +
                    "🟠 Medium Risk: Konkola, Chingola TSF\n" +
                    "🟢 Low Risk: Nchanga, Kitwe TSF\n\n" +
                    "Tap on any facility for detailed information.", "OK");
            }
            else
            {
                await DisplayAlert("Info", "Map is still loading. Please wait.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Show TSF failed: {ex.Message}", "OK");
        }
    }
}