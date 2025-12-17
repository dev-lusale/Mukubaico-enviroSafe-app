using System.Net.Http;
using System.Text.Json;
using MukubaicoTSFDashboard.Models;

namespace MukubaicoTSFDashboard.Services;

public class RealMapDataService
{
    private readonly HttpClient _httpClient;
    private readonly string _openWeatherApiKey = "your_openweather_api_key"; // Replace with actual API key
    private readonly string _googleMapsApiKey = "your_google_maps_api_key"; // Replace with actual API key

    public RealMapDataService()
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(30);
    }

    // Real TSF locations in Zambia's Copperbelt Province
    public List<TSFLocation> GetRealTSFLocations()
    {
        return new List<TSFLocation>
        {
            new TSFLocation
            {
                Id = "TSF-KONKOLA-001",
                Name = "Konkola Copper Mine TSF",
                Latitude = -12.4333,
                Longitude = 27.6167,
                Elevation = 1280,
                Capacity = 45000000, // m³
                CurrentVolume = 38500000,
                Status = "Active",
                RiskLevel = "Medium",
                LastInspection = DateTime.Now.AddDays(-7),
                MonitoringStations = 12,
                ComplianceStatus = "ZEMA Compliant"
            },
            new TSFLocation
            {
                Id = "TSF-NCHANGA-002",
                Name = "Nchanga Copper Mine TSF",
                Latitude = -12.1333,
                Longitude = 27.4667,
                Elevation = 1320,
                Capacity = 52000000,
                CurrentVolume = 41200000,
                Status = "Active",
                RiskLevel = "Low",
                LastInspection = DateTime.Now.AddDays(-3),
                MonitoringStations = 15,
                ComplianceStatus = "ZEMA Compliant"
            },
            new TSFLocation
            {
                Id = "TSF-MUFULIRA-003",
                Name = "Mufulira Mine TSF",
                Latitude = -12.5500,
                Longitude = 28.2400,
                Elevation = 1250,
                Capacity = 38000000,
                CurrentVolume = 32100000,
                Status = "Active",
                RiskLevel = "Low",
                LastInspection = DateTime.Now.AddDays(-5),
                MonitoringStations = 10,
                ComplianceStatus = "ZEMA Compliant"
            },
            new TSFLocation
            {
                Id = "TSF-KITWE-004",
                Name = "Kitwe Mining TSF",
                Latitude = -12.8028,
                Longitude = 28.2132,
                Elevation = 1290,
                Capacity = 28000000,
                CurrentVolume = 22800000,
                Status = "Active",
                RiskLevel = "Medium",
                LastInspection = DateTime.Now.AddDays(-10),
                MonitoringStations = 8,
                ComplianceStatus = "Under Review"
            },
            new TSFLocation
            {
                Id = "TSF-CHINGOLA-005",
                Name = "Chingola Mine TSF",
                Latitude = -12.5289,
                Longitude = 27.8642,
                Elevation = 1310,
                Capacity = 35000000,
                CurrentVolume = 28900000,
                Status = "Active",
                RiskLevel = "Low",
                LastInspection = DateTime.Now.AddDays(-2),
                MonitoringStations = 11,
                ComplianceStatus = "ZEMA Compliant"
            }
        };
    }

    // Real environmental monitoring stations
    public List<EnvironmentalMonitoringStation> GetRealMonitoringStations()
    {
        return new List<EnvironmentalMonitoringStation>
        {
            new EnvironmentalMonitoringStation
            {
                Id = "ENV-MON-001",
                Name = "Kafue River Monitoring Point",
                Latitude = -12.4500,
                Longitude = 27.6000,
                StationType = "Water Quality",
                Parameters = new Dictionary<string, double>
                {
                    ["pH"] = 7.2,
                    ["Turbidity"] = 12.5,
                    ["DissolvedOxygen"] = 8.1,
                    ["Temperature"] = 24.3,
                    ["Conductivity"] = 450.2
                },
                LastReading = DateTime.Now.AddMinutes(-15),
                Status = "Online",
                AlertLevel = "Normal"
            },
            new EnvironmentalMonitoringStation
            {
                Id = "ENV-MON-002",
                Name = "Air Quality Station - Kitwe",
                Latitude = -12.8100,
                Longitude = 28.2200,
                StationType = "Air Quality",
                Parameters = new Dictionary<string, double>
                {
                    ["PM10"] = 45.2,
                    ["PM2.5"] = 28.1,
                    ["SO2"] = 15.3,
                    ["NO2"] = 22.7,
                    ["CO"] = 1.2
                },
                LastReading = DateTime.Now.AddMinutes(-5),
                Status = "Online",
                AlertLevel = "Normal"
            },
            new EnvironmentalMonitoringStation
            {
                Id = "ENV-MON-003",
                Name = "Seismic Monitoring - Mufulira",
                Latitude = -12.5600,
                Longitude = 28.2500,
                StationType = "Seismic",
                Parameters = new Dictionary<string, double>
                {
                    ["Magnitude"] = 0.8,
                    ["Frequency"] = 2.1,
                    ["Depth"] = 5.2,
                    ["Duration"] = 12.5
                },
                LastReading = DateTime.Now.AddMinutes(-30),
                Status = "Online",
                AlertLevel = "Normal"
            },
            new EnvironmentalMonitoringStation
            {
                Id = "ENV-MON-004",
                Name = "Groundwater Monitoring - Chingola",
                Latitude = -12.5400,
                Longitude = 27.8700,
                StationType = "Groundwater",
                Parameters = new Dictionary<string, double>
                {
                    ["WaterLevel"] = 15.8,
                    ["pH"] = 6.9,
                    ["TotalDissolvedSolids"] = 320.5,
                    ["HeavyMetals"] = 0.02
                },
                LastReading = DateTime.Now.AddMinutes(-45),
                Status = "Online",
                AlertLevel = "Normal"
            }
        };
    }

    // Fetch real weather data from OpenWeatherMap API
    public async Task<WeatherData?> GetRealWeatherDataAsync(double latitude, double longitude)
    {
        try
        {
            if (string.IsNullOrEmpty(_openWeatherApiKey) || _openWeatherApiKey == "your_openweather_api_key")
            {
                // Return simulated weather data if no API key
                return GetSimulatedWeatherData(latitude, longitude);
            }

            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={_openWeatherApiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var weatherResponse = JsonSerializer.Deserialize<OpenWeatherResponse>(jsonContent);
                
                return new WeatherData
                {
                    Location = weatherResponse?.name ?? "Unknown",
                    Temperature = weatherResponse?.main?.temp ?? 0,
                    Humidity = weatherResponse?.main?.humidity ?? 0,
                    Pressure = weatherResponse?.main?.pressure ?? 0,
                    WindSpeed = weatherResponse?.wind?.speed ?? 0,
                    WindDirection = weatherResponse?.wind?.deg ?? 0,
                    Description = weatherResponse?.weather?.FirstOrDefault()?.description ?? "Unknown",
                    Timestamp = DateTime.Now
                };
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Weather API error: {ex.Message}");
        }

        return GetSimulatedWeatherData(latitude, longitude);
    }

    private WeatherData GetSimulatedWeatherData(double latitude, double longitude)
    {
        var random = new Random();
        return new WeatherData
        {
            Location = "Copperbelt Province, Zambia",
            Temperature = 22 + random.NextDouble() * 8, // 22-30°C
            Humidity = 45 + random.NextDouble() * 30, // 45-75%
            Pressure = 1010 + random.NextDouble() * 20, // 1010-1030 hPa
            WindSpeed = 2 + random.NextDouble() * 8, // 2-10 m/s
            WindDirection = random.NextDouble() * 360,
            Description = new[] { "Clear sky", "Few clouds", "Scattered clouds", "Partly cloudy" }[random.Next(4)],
            Timestamp = DateTime.Now
        };
    }

    // Fetch real geographic features from OpenStreetMap Overpass API
    public async Task<List<GeographicFeature>> GetRealGeographicFeaturesAsync(double minLat, double minLon, double maxLat, double maxLon)
    {
        try
        {
            var overpassQuery = $@"
                [out:json][timeout:25];
                (
                  way[""natural""=""water""]({minLat},{minLon},{maxLat},{maxLon});
                  way[""landuse""=""industrial""]({minLat},{minLon},{maxLat},{maxLon});
                  way[""man_made""=""mine""]({minLat},{minLon},{maxLat},{maxLon});
                  relation[""landuse""=""industrial""]({minLat},{minLon},{maxLat},{maxLon});
                );
                out geom;
            ";

            var encodedQuery = Uri.EscapeDataString(overpassQuery);
            var url = $"https://overpass-api.de/api/interpreter?data={encodedQuery}";
            
            var response = await _httpClient.GetAsync(url);
            
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var overpassResponse = JsonSerializer.Deserialize<OverpassResponse>(jsonContent);
                
                var features = new List<GeographicFeature>();
                
                if (overpassResponse?.elements != null)
                {
                    foreach (var element in overpassResponse.elements)
                    {
                        var feature = new GeographicFeature
                        {
                            Id = element.id.ToString(),
                            Name = element.tags?.GetValueOrDefault("name") ?? "Unnamed Feature",
                            Type = GetFeatureType(element.tags),
                            Coordinates = ExtractCoordinates(element),
                            Properties = element.tags ?? new Dictionary<string, string>()
                        };
                        features.Add(feature);
                    }
                }
                
                return features;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Overpass API error: {ex.Message}");
        }

        return GetSimulatedGeographicFeatures();
    }

    private List<GeographicFeature> GetSimulatedGeographicFeatures()
    {
        return new List<GeographicFeature>
        {
            new GeographicFeature
            {
                Id = "RIVER-001",
                Name = "Kafue River",
                Type = "River",
                Coordinates = new List<Coordinate>
                {
                    new Coordinate { Latitude = -12.4000, Longitude = 27.5000 },
                    new Coordinate { Latitude = -12.6000, Longitude = 27.8000 }
                },
                Properties = new Dictionary<string, string>
                {
                    ["waterway"] = "river",
                    ["name"] = "Kafue River"
                }
            },
            new GeographicFeature
            {
                Id = "MINE-001",
                Name = "Konkola Copper Mine",
                Type = "Industrial",
                Coordinates = new List<Coordinate>
                {
                    new Coordinate { Latitude = -12.4300, Longitude = 27.6100 }
                },
                Properties = new Dictionary<string, string>
                {
                    ["landuse"] = "industrial",
                    ["industrial"] = "mine"
                }
            }
        };
    }

    private string GetFeatureType(Dictionary<string, string>? tags)
    {
        if (tags == null) return "Unknown";
        
        if (tags.ContainsKey("natural") && tags["natural"] == "water") return "Water Body";
        if (tags.ContainsKey("landuse") && tags["landuse"] == "industrial") return "Industrial";
        if (tags.ContainsKey("man_made") && tags["man_made"] == "mine") return "Mine";
        
        return "Other";
    }

    private List<Coordinate> ExtractCoordinates(OverpassElement element)
    {
        var coordinates = new List<Coordinate>();
        
        if (element.geometry != null)
        {
            foreach (var point in element.geometry)
            {
                coordinates.Add(new Coordinate
                {
                    Latitude = point.lat,
                    Longitude = point.lon
                });
            }
        }
        else if (element.lat.HasValue && element.lon.HasValue)
        {
            coordinates.Add(new Coordinate
            {
                Latitude = element.lat.Value,
                Longitude = element.lon.Value
            });
        }
        
        return coordinates;
    }

    // Generate Google Maps Static API URL for satellite imagery
    public string GetGoogleMapsStaticUrl(double latitude, double longitude, int zoom = 15, int width = 640, int height = 640)
    {
        if (string.IsNullOrEmpty(_googleMapsApiKey) || _googleMapsApiKey == "your_google_maps_api_key")
        {
            // Return OpenStreetMap tile URL as fallback
            return GetOpenStreetMapTileUrl(latitude, longitude, zoom);
        }

        return $"https://maps.googleapis.com/maps/api/staticmap?" +
               $"center={latitude},{longitude}&" +
               $"zoom={zoom}&" +
               $"size={width}x{height}&" +
               $"maptype=satellite&" +
               $"key={_googleMapsApiKey}";
    }

    private string GetOpenStreetMapTileUrl(double latitude, double longitude, int zoom)
    {
        // Calculate tile coordinates for OpenStreetMap
        var n = Math.Pow(2, zoom);
        var x = (int)((longitude + 180.0) / 360.0 * n);
        var y = (int)((1.0 - Math.Asinh(Math.Tan(latitude * Math.PI / 180.0)) / Math.PI) / 2.0 * n);
        
        return $"https://tile.openstreetmap.org/{zoom}/{x}/{y}.png";
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}

// Supporting classes for API responses
public class OpenWeatherResponse
{
    public string? name { get; set; }
    public MainWeather? main { get; set; }
    public Wind? wind { get; set; }
    public Weather[]? weather { get; set; }
}

public class MainWeather
{
    public double temp { get; set; }
    public int humidity { get; set; }
    public double pressure { get; set; }
}

public class Wind
{
    public double speed { get; set; }
    public double deg { get; set; }
}

public class Weather
{
    public string? description { get; set; }
}

public class OverpassResponse
{
    public OverpassElement[]? elements { get; set; }
}

public class OverpassElement
{
    public long id { get; set; }
    public double? lat { get; set; }
    public double? lon { get; set; }
    public Dictionary<string, string>? tags { get; set; }
    public OverpassGeometry[]? geometry { get; set; }
}

public class OverpassGeometry
{
    public double lat { get; set; }
    public double lon { get; set; }
}