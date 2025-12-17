# MUKUBAICO ENVIRO-SAFE - TSF Safety & Compliance Dashboard

## Overview
Professional, interactive Tailings Storage Facility (TSF) Safety & Compliance Dashboard for Mukubaico Enviro-Safe, integrating Zambian legal requirements, international mining standards, and real-time monitoring for environmental and structural compliance.

## Features

### 1. Header Section
- **Title**: MUKUBAICO ENVIRO-SAFE – TSF Safety & Compliance Dashboard
- **Standards Badges**: ZEMA, EIA, GISTM, ICOLD, IFC, ISO, MINE SAFETY (ZM)

### 2. Left Panel - GIS Map (Visual)
- **Purpose**: Visualize spatial compliance with environmental and safety regulations
- **Elements**: 
  - TSF boundary
  - Rivers/streams
  - Groundwater wells
  - Community safety buffers (500m)
  - Protected vegetation zones
- **Standards**: ZEMA, EIA Regulations, IFC EHS spatial planning

### 3. Right Panel - Live Risk Status
- **Real-time Risk Score**: 0-100 scale
- **Alert Levels**: 
  - Green (0-39): Low Risk
  - Amber (40-69): Medium Risk
  - Red (70-100): High Risk
- **Last Update**: Real-time timestamp
- **Refresh**: Manual and auto-refresh every 5 seconds

### 4. Compliance Panels by Standard

#### ZEMA (Environmental Management Act, No. 12 of 2011)
- Water quality monitoring (pH, turbidity)
- Groundwater well operations
- Buffer zone compliance (500m)

#### EIA (Environmental Impact Assessment Regulations, S.I. No. 28 of 1997)
- Mitigation measures implementation
- Buffer conditions monitoring
- Rehabilitation plans status

#### GISTM (Global Industry Standard on Tailings Management, 2020)
- Risk governance and board oversight
- Real-time IoT monitoring systems
- Emergency readiness and drills

#### ICOLD (Tailings Dam Safety Bulletins)
- Structural integrity monitoring
- Seepage control (flow rates)
- Deformation monitoring (inclinometers)

#### IFC EHS (Environmental, Health and Safety Guidelines for Mining)
- Discharge quality (TSS levels)
- Dust suppression (PM10 monitoring)
- Waste handling compliance

#### ISO Standards (14001, 45001, 31000)
- ISO 14001: Environmental management
- ISO 45001: Occupational health and safety
- ISO 31000: Risk management

#### Mine Safety - Zambia (Mines and Minerals Development Act, No. 11 of 2015)
- Restricted zones management
- Emergency systems testing
- Safety training certification
- 24-hour incident reporting

### 5. Summary Compliance Radar
- Visual representation of all standards compliance
- Progress bars for each standard
- Overall compliance threshold monitoring (90%+)

## Technical Stack
- **.NET 9.0**
- **.NET MAUI** (Multi-platform App UI)
- **MVVM Architecture**
- **Real-time Data Binding**
- **Auto-refresh Capability**

## Project Structure
```
MukubaicoTSFDashboard/
├── Models/
│   └── TSFData.cs              # Data models for TSF and compliance
├── Services/
│   └── TSFDataService.cs       # Data service with simulated IoT data
├── ViewModels/
│   └── DashboardViewModel.cs   # Main dashboard view model
├── Views/
│   └── ComplianceDetailPage.xaml  # Detailed compliance view
├── Converters/
│   └── PercentToDecimalConverter.cs  # Value converter for progress bars
├── MainPage.xaml               # Main dashboard UI
└── App.xaml                    # Application resources
```

## Running the Application

### Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 or later (with .NET MAUI workload)
- Windows 10/11 for Windows development

### Build and Run
```bash
cd MukubaicoTSFDashboard
dotnet build
dotnet run
```

### Platform-Specific Commands
**Windows:**
```bash
dotnet build -f net9.0-windows10.0.19041.0
```

**Android:**
```bash
dotnet build -f net9.0-android
```

**iOS:**
```bash
dotnet build -f net9.0-ios
```

**macOS:**
```bash
dotnet build -f net9.0-maccatalyst
```

## Features Implementation

### Real-time Monitoring
- Auto-refresh every 5 seconds
- Live risk score calculation
- Dynamic alert level updates
- IoT sensor data integration ready

### Compliance Tracking
- Seven major standards monitored
- Individual compliance percentages
- Status indicators (✅, ⚠, ❌)
- Plain-language explanations

### Professional UI
- Clean, modern design
- Color-coded risk levels
- Modular, interactive panels
- Responsive layout

## Legal & Regulatory References

1. **Environmental Management Act, No. 12 of 2011 (ZEMA)**
2. **Environmental Impact Assessment Regulations, S.I. No. 28 of 1997**
3. **Mines and Minerals Development Act, No. 11 of 2015**
4. **Mine Safety Department (MSD) Regulations and Guidelines**
5. **Global Industry Standard on Tailings Management (GISTM, 2020 – ICMM/UNEP/PRI)**
6. **ICOLD Tailings Dam Safety Bulletins**
7. **IFC Environmental, Health and Safety (EHS) Guidelines for Mining**
8. **ISO 14001** (Environmental Management Systems)
9. **ISO 45001** (Occupational Health and Safety Management Systems)
10. **ISO 31000** (Risk Management)

## Future Enhancements
- Integration with actual IoT sensors
- GIS map integration with real coordinates
- Historical data trending and analytics
- Export compliance reports (PDF/Excel)
- Multi-facility support
- Alert notifications and email reports
- Mobile app deployment (Android/iOS)
- Offline mode with data synchronization

## Support
For technical support or questions, contact the development team.

## License
Proprietary - Mukubaico Enviro-Safe

---
**Dashboard Version**: 1.0.0  
**Last Updated**: December 2025
