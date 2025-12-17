namespace MukubaicoTSFDashboard.Models;

public class QGISLayerData
{
    public string LayerId { get; set; } = "";
    public string LayerName { get; set; } = "";
    public string LayerType { get; set; } = ""; // Vector, Raster, 3D
    public string GeometryType { get; set; } = ""; // Point, Line, Polygon
    public Dictionary<string, object> Attributes { get; set; } = new();
    public QGISCoordinate Coordinate { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}

public class QGISCoordinate
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Elevation { get; set; }
    public string CoordinateSystem { get; set; } = "EPSG:4326"; // WGS84
}

public class QGIS3DMapData
{
    public string MapId { get; set; } = "";
    public string ProjectName { get; set; } = "";
    public List<QGISLayerData> Layers { get; set; } = new();
    public QGISExtent MapExtent { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
    public QGISAnalysisResults AnalysisResults { get; set; } = new();
}

public class QGISExtent
{
    public double MinX { get; set; }
    public double MaxX { get; set; }
    public double MinY { get; set; }
    public double MaxY { get; set; }
    public double MinZ { get; set; }
    public double MaxZ { get; set; }
}

public class QGISAnalysisResults
{
    public Dictionary<string, double> VolumeCalculations { get; set; } = new();
    public Dictionary<string, double> StabilityAnalysis { get; set; } = new();
    public Dictionary<string, string> RiskAssessment { get; set; } = new();
    public List<QGISAlert> Alerts { get; set; } = new();
}

public class QGISAlert
{
    public string AlertId { get; set; } = "";
    public string AlertType { get; set; } = ""; // Warning, Critical, Info
    public string Message { get; set; } = "";
    public QGISCoordinate Location { get; set; } = new();
    public DateTime Timestamp { get; set; }
    public string Severity { get; set; } = ""; // Low, Medium, High, Critical
}

public class QGISDataPacket
{
    public string ProjectId { get; set; } = "";
    public string ProjectName { get; set; } = "";
    public DateTime Timestamp { get; set; }
    public QGIS3DMapData MapData { get; set; } = new();
    public List<QGISLayerData> UpdatedLayers { get; set; } = new();
    public QGISAnalysisResults LatestAnalysis { get; set; } = new();
}