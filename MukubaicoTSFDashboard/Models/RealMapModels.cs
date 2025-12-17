namespace MukubaicoTSFDashboard.Models;

public class TSFLocation
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Elevation { get; set; }
    public double Capacity { get; set; } // m³
    public double CurrentVolume { get; set; } // m³
    public string Status { get; set; } = ""; // Active, Inactive, Under Construction
    public string RiskLevel { get; set; } = ""; // Low, Medium, High, Critical
    public DateTime LastInspection { get; set; }
    public int MonitoringStations { get; set; }
    public string ComplianceStatus { get; set; } = "";
    
    public double CapacityUtilization => CurrentVolume / Capacity * 100;
    public bool IsOverCapacity => CurrentVolume > Capacity;
    public int DaysSinceInspection => (DateTime.Now - LastInspection).Days;
}

public class EnvironmentalMonitoringStation
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string StationType { get; set; } = ""; // Water Quality, Air Quality, Seismic, Groundwater
    public Dictionary<string, double> Parameters { get; set; } = new();
    public DateTime LastReading { get; set; }
    public string Status { get; set; } = ""; // Online, Offline, Maintenance
    public string AlertLevel { get; set; } = ""; // Normal, Warning, Critical
}

public class WeatherData
{
    public string Location { get; set; } = "";
    public double Temperature { get; set; } // °C
    public double Humidity { get; set; } // %
    public double Pressure { get; set; } // hPa
    public double WindSpeed { get; set; } // m/s
    public double WindDirection { get; set; } // degrees
    public string Description { get; set; } = "";
    public DateTime Timestamp { get; set; }
}

public class GeographicFeature
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Type { get; set; } = ""; // River, Industrial, Mine, etc.
    public List<Coordinate> Coordinates { get; set; } = new();
    public Dictionary<string, string> Properties { get; set; } = new();
}

public class Coordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Elevation { get; set; }
}

public class RealTimeAlert
{
    public string Id { get; set; } = "";
    public string Type { get; set; } = ""; // Environmental, Safety, Compliance
    public string Severity { get; set; } = ""; // Low, Medium, High, Critical
    public string Message { get; set; } = "";
    public string Source { get; set; } = ""; // Station ID or System
    public Coordinate Location { get; set; } = new();
    public DateTime Timestamp { get; set; }
    public bool IsAcknowledged { get; set; }
    public string? AcknowledgedBy { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
}

public class SpatialAnalysisResult
{
    public string AnalysisId { get; set; } = "";
    public string TSFLocationId { get; set; } = "";
    public DateTime AnalysisDate { get; set; }
    public VolumeAnalysis VolumeData { get; set; } = new();
    public StabilityAnalysis StabilityData { get; set; } = new();
    public EnvironmentalImpact EnvironmentalData { get; set; } = new();
    public RiskAssessment RiskData { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
}

public class VolumeAnalysis
{
    public double TotalVolume { get; set; } // m³
    public double WaterVolume { get; set; } // m³
    public double SolidVolume { get; set; } // m³
    public double VolumeChange { get; set; } // % change from previous analysis
    public double ProjectedCapacity { get; set; } // months until full
}

public class StabilityAnalysis
{
    public double SlopeStabilityFactor { get; set; }
    public double SettlementRate { get; set; } // mm/year
    public double SeepageRate { get; set; } // L/min
    public double PoreWaterPressure { get; set; } // kPa
    public string StabilityStatus { get; set; } = ""; // Stable, Marginal, Unstable
}

public class EnvironmentalImpact
{
    public double GroundwaterContamination { get; set; } // mg/L
    public double AirQualityIndex { get; set; }
    public double NoiseLevel { get; set; } // dB
    public double DustEmission { get; set; } // mg/m³
    public string ComplianceStatus { get; set; } = "";
}

public class RiskAssessment
{
    public string OverallRisk { get; set; } = "";
    public double RiskScore { get; set; } // 0-100
    public Dictionary<string, string> RiskFactors { get; set; } = new();
    public List<string> MitigationMeasures { get; set; } = new();
    public DateTime NextAssessmentDue { get; set; }
}

public class MapCaptureResult
{
    public bool Success { get; set; }
    public string? ImagePath { get; set; }
    public string? FileName { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public long FileSizeBytes { get; set; }
    public DateTime CaptureTime { get; set; }
    public Coordinate CenterPoint { get; set; } = new();
    public int ZoomLevel { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ReportGenerationResult
{
    public bool Success { get; set; }
    public string? ReportPath { get; set; }
    public string? FileName { get; set; }
    public string ReportType { get; set; } = ""; // PDF, Excel, Word
    public int PageCount { get; set; }
    public long FileSizeBytes { get; set; }
    public DateTime GenerationTime { get; set; }
    public List<string> IncludedSections { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class DataExportResult
{
    public bool Success { get; set; }
    public string? ExportPath { get; set; }
    public List<string> ExportedFormats { get; set; } = new(); // GeoJSON, Shapefile, KML, CSV
    public int FileCount { get; set; }
    public long TotalSizeBytes { get; set; }
    public DateTime ExportTime { get; set; }
    public Dictionary<string, string> FileManifest { get; set; } = new(); // Format -> FilePath
    public string? ErrorMessage { get; set; }
}