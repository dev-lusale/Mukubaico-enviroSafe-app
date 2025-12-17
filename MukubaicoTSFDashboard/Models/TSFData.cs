namespace MukubaicoTSFDashboard.Models;

public class TSFData
{
    public string FacilityId { get; set; } = "TSF-01";
    public double RiskScore { get; set; }
    public string AlertLevel { get; set; } = "Green";
    public DateTime LastUpdate { get; set; } = DateTime.Now;
    public ComplianceData Compliance { get; set; } = new();
}

public class ComplianceData
{
    public StandardCompliance ZEMA { get; set; } = new();
    public StandardCompliance EIA { get; set; } = new();
    public StandardCompliance GISTM { get; set; } = new();
    public StandardCompliance ICOLD { get; set; } = new();
    public StandardCompliance IFC { get; set; } = new();
    public StandardCompliance ISO { get; set; } = new();
    public StandardCompliance MineSafety { get; set; } = new();
}

public class StandardCompliance
{
    public string Name { get; set; } = "";
    public double CompliancePercentage { get; set; }
    public List<ComplianceItem> Items { get; set; } = new();
}

public class ComplianceItem
{
    public string Name { get; set; } = "";
    public string Status { get; set; } = "✅";
    public string Description { get; set; } = "";
    public double Value { get; set; }
    public string Unit { get; set; } = "";
}
