using MukubaicoTSFDashboard.Models;

namespace MukubaicoTSFDashboard.Services;

public class TSFDataService
{
    private readonly Random _random = new();

    public TSFData GetLiveData()
    {
        var riskScore = _random.Next(15, 85);
        var alertLevel = riskScore < 40 ? "Green" : riskScore < 70 ? "Amber" : "Red";

        return new TSFData
        {
            FacilityId = "TSF-01",
            RiskScore = riskScore,
            AlertLevel = alertLevel,
            LastUpdate = DateTime.Now,
            Compliance = new ComplianceData
            {
                ZEMA = CreateZEMACompliance(),
                EIA = CreateEIACompliance(),
                GISTM = CreateGISTMCompliance(),
                ICOLD = CreateICOLDCompliance(),
                IFC = CreateIFCCompliance(),
                ISO = CreateISOCompliance(),
                MineSafety = CreateMineSafetyCompliance()
            }
        };
    }

    private StandardCompliance CreateZEMACompliance()
    {
        var items = new List<ComplianceItem>
        {
            new() { Name = "Water Quality", Status = "✅", Description = "pH, turbidity within limits", Value = 7.2, Unit = "pH" },
            new() { Name = "Groundwater Monitoring", Status = "✅", Description = "All wells operational", Value = 98, Unit = "%" },
            new() { Name = "Buffer Zones", Status = "✅", Description = "500m buffer maintained", Value = 500, Unit = "m" }
        };
        return new StandardCompliance { Name = "ZEMA", CompliancePercentage = 96, Items = items };
    }

    private StandardCompliance CreateEIACompliance()
    {
        var items = new List<ComplianceItem>
        {
            new() { Name = "Mitigation Measures", Status = "✅", Description = "All measures implemented", Value = 100, Unit = "%" },
            new() { Name = "Buffer Conditions", Status = "⚠", Description = "Minor vegetation loss", Value = 88, Unit = "%" },
            new() { Name = "Rehabilitation Plans", Status = "✅", Description = "Plans updated quarterly", Value = 95, Unit = "%" }
        };
        return new StandardCompliance { Name = "EIA", CompliancePercentage = 94, Items = items };
    }

    private StandardCompliance CreateGISTMCompliance()
    {
        var items = new List<ComplianceItem>
        {
            new() { Name = "Risk Governance", Status = "✅", Description = "Board oversight active", Value = 100, Unit = "%" },
            new() { Name = "Monitoring Systems", Status = "✅", Description = "Real-time IoT sensors", Value = 97, Unit = "%" },
            new() { Name = "Emergency Readiness", Status = "✅", Description = "Drills conducted monthly", Value = 92, Unit = "%" }
        };
        return new StandardCompliance { Name = "GISTM", CompliancePercentage = 96, Items = items };
    }

    private StandardCompliance CreateICOLDCompliance()
    {
        var items = new List<ComplianceItem>
        {
            new() { Name = "Structural Integrity", Status = "✅", Description = "No deformation detected", Value = 99, Unit = "%" },
            new() { Name = "Seepage Control", Status = "✅", Description = "Flow rates normal", Value = 0.5, Unit = "L/s" },
            new() { Name = "Deformation Monitoring", Status = "✅", Description = "Inclinometers stable", Value = 2.1, Unit = "mm" }
        };
        return new StandardCompliance { Name = "ICOLD", CompliancePercentage = 98, Items = items };
    }

    private StandardCompliance CreateIFCCompliance()
    {
        var items = new List<ComplianceItem>
        {
            new() { Name = "Discharge Quality", Status = "✅", Description = "TSS below 50 mg/L", Value = 32, Unit = "mg/L" },
            new() { Name = "Dust Suppression", Status = "✅", Description = "PM10 within limits", Value = 45, Unit = "μg/m³" },
            new() { Name = "Waste Handling", Status = "✅", Description = "Zero spills this month", Value = 100, Unit = "%" }
        };
        return new StandardCompliance { Name = "IFC EHS", CompliancePercentage = 97, Items = items };
    }

    private StandardCompliance CreateISOCompliance()
    {
        var items = new List<ComplianceItem>
        {
            new() { Name = "ISO 14001 (Environmental)", Status = "✅", Description = "Certified and audited", Value = 95, Unit = "%" },
            new() { Name = "ISO 45001 (Safety)", Status = "✅", Description = "Zero LTI this quarter", Value = 100, Unit = "%" },
            new() { Name = "ISO 31000 (Risk)", Status = "✅", Description = "Risk register updated", Value = 93, Unit = "%" }
        };
        return new StandardCompliance { Name = "ISO", CompliancePercentage = 96, Items = items };
    }

    private StandardCompliance CreateMineSafetyCompliance()
    {
        var items = new List<ComplianceItem>
        {
            new() { Name = "Restricted Zones", Status = "✅", Description = "Signage and barriers intact", Value = 100, Unit = "%" },
            new() { Name = "Emergency Systems", Status = "✅", Description = "Alarms tested weekly", Value = 98, Unit = "%" },
            new() { Name = "Safety Training", Status = "✅", Description = "All staff certified", Value = 100, Unit = "%" },
            new() { Name = "Incident Reporting", Status = "✅", Description = "24hr reporting active", Value = 100, Unit = "%" }
        };
        return new StandardCompliance { Name = "Mine Safety (ZM)", CompliancePercentage = 99, Items = items };
    }
}
