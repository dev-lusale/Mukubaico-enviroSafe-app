# Quick Start Guide - TSF Safety & Compliance Dashboard

## Prerequisites
- .NET 9.0 SDK installed
- Visual Studio 2022 (recommended) or VS Code with C# Dev Kit
- Windows 10/11 for Windows development

## Running the Application

### Option 1: Using Visual Studio 2022
1. Open `MukubaicoTSFDashboard.sln` or `MukubaicoTSFDashboard.csproj`
2. Select target platform (Windows Machine)
3. Press F5 or click "Run" button
4. The dashboard will launch automatically

### Option 2: Using Command Line (Windows)
```bash
cd MukubaicoTSFDashboard
dotnet build -f net9.0-windows10.0.19041.0
dotnet run -f net9.0-windows10.0.19041.0
```

### Option 3: Using VS Code
1. Open the `MukubaicoTSFDashboard` folder
2. Press F5 or use "Run and Debug" panel
3. Select ".NET MAUI" configuration

## Troubleshooting

### Build Issues
If you encounter build errors:
```bash
# Clean the project
dotnet clean

# Restore packages
dotnet restore

# Rebuild
dotnet build
```

### File Lock Issues
If you see "file is being used by another process":
1. Close Visual Studio or any running instances
2. Delete `bin` and `obj` folders
3. Rebuild the project

### Missing Workloads
If .NET MAUI workload is not installed:
```bash
dotnet workload install maui
```

## Dashboard Features

### Real-time Updates
- Dashboard auto-refreshes every 5 seconds
- Manual refresh button available
- Live risk score calculation

### Compliance Standards Monitored
1. **ZEMA** - Environmental Management Act (Zambia)
2. **EIA** - Environmental Impact Assessment
3. **GISTM** - Global Industry Standard on Tailings Management
4. **ICOLD** - International Commission on Large Dams
5. **IFC EHS** - IFC Environmental, Health & Safety Guidelines
6. **ISO** - ISO 14001, 45001, 31000
7. **Mine Safety (ZM)** - Zambian Mine Safety Regulations

### GIS Map Legend
- **⬢ TSF Boundary** - Tailings Storage Facility perimeter
- **≋ Rivers/Streams** - Water bodies near facility
- **● Groundwater Wells** - Monitoring well locations
- **◉ Community Safety Buffer** - 500m safety zone
- **▲ Protected Vegetation** - Environmental protection zones

### Risk Alert Levels
- **Green (0-39)**: Low Risk - All systems normal
- **Amber (40-69)**: Medium Risk - Attention required
- **Red (70-100)**: High Risk - Immediate action needed

## Customization

### Connecting Real IoT Sensors
Edit `Services/TSFDataService.cs` to integrate with your actual IoT data sources:
```csharp
public TSFData GetLiveData()
{
    // Replace with actual API calls to your IoT platform
    // Example: var data = await _iotClient.GetSensorDataAsync();
}
```

### Adjusting Refresh Rate
Edit `ViewModels/DashboardViewModel.cs`:
```csharp
private async void StartAutoRefresh()
{
    while (true)
    {
        await Task.Delay(5000); // Change 5000 to desired milliseconds
        // ...
    }
}
```

### Adding More Facilities
Extend the `TSFData` model to support multiple facilities and add a facility selector in the UI.

## Support & Documentation
- Full documentation: See `README.md`
- Legal references: All Zambian and international standards documented
- Architecture: MVVM pattern with data binding

## Next Steps
1. ✅ Run the application
2. ✅ Review the dashboard layout
3. ✅ Test the refresh functionality
4. ⬜ Integrate with real IoT sensors
5. ⬜ Connect to actual GIS mapping service
6. ⬜ Deploy to production environment

---
**Version**: 1.0.0  
**Platform**: .NET MAUI 9.0  
**Last Updated**: December 2025
