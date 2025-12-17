# TSF Safety & Compliance Dashboard - User Guide

## Table of Contents
1. [Getting Started](#getting-started)
2. [Login](#login)
3. [Main Dashboard](#main-dashboard)
4. [Compliance Pages](#compliance-pages)
5. [Arduino Integration](#arduino-integration)
6. [Navigation](#navigation)
7. [User Roles](#user-roles)
8. [Troubleshooting](#troubleshooting)

## Getting Started

### System Requirements
- Windows 10/11 (64-bit)
- .NET 9.0 Runtime
- 4GB RAM minimum
- 1920x1080 screen resolution recommended
- USB port for Arduino connection (optional)

### First Launch
1. Double-click the application icon
2. Wait for the login screen to appear
3. Use demo credentials to log in

## Login

### Login Screen
The login screen is the first page you'll see when launching the application.

**Demo Credentials:**
- **Admin**: username: `admin` / password: `admin123`
- **Operator**: username: `operator` / password: `operator123`
- **Viewer**: username: `viewer` / password: `viewer123`

### Login Process
1. Enter your username
2. Enter your password
3. Click the "LOGIN" button
4. Wait for authentication (takes ~1 second)
5. You'll be redirected to the Main Dashboard

### Troubleshooting Login
- **Invalid credentials**: Double-check username and password
- **Loading forever**: Restart the application
- **Error message**: Contact system administrator

## Main Dashboard

### Overview
The Main Dashboard provides a comprehensive view of all compliance standards at a glance.

### Dashboard Sections

#### 1. Header
- Company branding: MUKUBAICO ENVIRO-SAFE
- Dashboard title
- 7 compliance standard badges

#### 2. GIS Spatial Compliance Map (Left Panel)
- Visual representation of TSF boundaries
- Rivers and streams
- Groundwater wells
- Community safety buffer (500m)
- Protected vegetation zones

**Legend:**
- ⬢ TSF Boundary (Blue)
- ≋ Rivers/Streams (Light Blue)
- ● Groundwater Wells (Cyan)
- ◉ Community Safety Buffer (Teal)
- ▲ Protected Vegetation Zones (Green)

#### 3. Live Risk Status (Right Panel)
- **Facility ID**: TSF-01
- **Risk Score**: 0-100 scale
- **Alert Level**: 
  - Green (0-39): Low Risk
  - Amber (40-69): Medium Risk
  - Red (70-100): High Risk
- **Last Update**: Real-time timestamp
- **Refresh Button**: Manual data refresh

#### 4. Compliance Panels
Seven panels showing compliance for each standard:
- ZEMA (96%)
- EIA (94%)
- GISTM (96%)
- ICOLD (98%)
- IFC EHS (97%)
- ISO (96%)
- Mine Safety - Zambia (99%)

Each panel shows:
- Compliance percentage
- Status indicators (✅, ⚠, ❌)
- Key metrics
- Plain-language descriptions

#### 5. Summary Compliance Radar
- Progress bars for all 7 standards
- Overall compliance threshold (90%+)
- Visual compliance summary

### Auto-Refresh
- Dashboard automatically refreshes every 5 seconds
- Manual refresh available via button
- Real-time data from Arduino sensors

## Compliance Pages

### Accessing Compliance Pages
1. Click the menu icon (☰) in the top-left corner
2. Navigate to "Compliance Standards"
3. Select the standard you want to view

### ZEMA Page
**Environmental Management Act, No. 12 of 2011**

**Sections:**
- Overall Compliance Score (96%)
- Water Quality Monitoring
  - pH Levels (7.2 pH)
  - Turbidity (18 NTU)
- Groundwater Monitoring
  - Well Status (3/3 active)
- Buffer Zones Compliance (500m)
- Real-time Arduino Sensor Data

**Actions:**
- View Reports
- Refresh Data

### EIA Page
**Environmental Impact Assessment Regulations**

**Sections:**
- Overall Compliance Score (94%)
- Mitigation Measures
  - Dust Control Systems
  - Water Management
  - Erosion Control
- Buffer Conditions (88% vegetation cover)
- Rehabilitation Plans (95% compliance)

### GISTM Page
**Global Industry Standard on Tailings Management**

**Sections:**
- Overall Compliance Score (96%)
- Risk Governance
  - Board Oversight
  - Risk Register
- Monitoring Systems
  - IoT Sensors (97% online)
  - Arduino Integration
- Emergency Readiness
  - Monthly Drills
  - Response Plans

### ICOLD Page
**International Commission on Large Dams**

**Sections:**
- Overall Compliance Score (98%)
- Structural Integrity Monitoring
  - Inclinometer Readings (2.1mm)
- Seepage Control (0.5 L/s)
- Pore Pressure Monitoring (52 kPa)
- Arduino Sensor Status

### IFC EHS Page
**IFC Environmental, Health and Safety Guidelines**

**Sections:**
- Overall Compliance Score (97%)
- Discharge Quality
  - TSS Levels (32 mg/L)
- Dust Suppression
  - PM10 Monitoring (45 μg/m³)
- Waste Handling
  - Zero Spills Record

### ISO Page
**International Organization for Standardization**

**Sections:**
- Overall Compliance Score (96%)
- ISO 14001 (Environmental Management) - 95%
- ISO 45001 (Occupational Safety) - 100%
- ISO 31000 (Risk Management) - 93%

### Mine Safety - Zambia Page
**Mines and Minerals Development Act, No. 11 of 2015**

**Sections:**
- Overall Compliance Score (99%)
- Restricted Zones Management
- Emergency Systems (8/8 operational)
- Safety Training (100% certified)
- Incident Reporting (24-hour active)

## Arduino Integration

### Connection Status
- Check for "🔌 Arduino Connected" indicator
- Device ID: ARDUINO-TSF-01
- Last Update timestamp

### Sensor Data
Real-time data from Arduino sensors:
- pH Sensor (PH-01)
- Turbidity Sensor (TURB-01)
- TSS Sensor (TSS-01)
- Inclinometer (INCL-01)
- Seepage Meter (SEEP-01)
- Pressure Sensor (PRES-01)
- PM10 Sensor (PM10-01)
- Water Level Sensors (GW-01, GW-02)

### Data Refresh Rate
- Automatic: Every 5 seconds
- Manual: Click "Refresh Data" button

### Troubleshooting Arduino
- **No connection**: Check USB cable
- **No data**: Verify COM port settings
- **Incorrect readings**: Calibrate sensors
- See ARDUINO_INTEGRATION.md for details

## Navigation

### Menu Structure
```
☰ Menu
├── Dashboard
│   └── Main Dashboard
├── Compliance Standards
│   ├── ZEMA
│   ├── EIA
│   ├── GISTM
│   ├── ICOLD
│   ├── IFC EHS
│   ├── ISO
│   └── Mine Safety (ZM)
└── Logout
```

### Navigation Tips
- Use the hamburger menu (☰) to access all pages
- Click "Back" to return to previous page
- Use "Home" icon to return to Main Dashboard
- Logout from the menu when finished

## User Roles

### Admin
**Permissions:**
- Full access to all features
- View all compliance data
- Generate reports
- Modify settings
- Manage users

### Operator
**Permissions:**
- View all compliance data
- Refresh data manually
- Generate reports
- Limited settings access

### Viewer
**Permissions:**
- View-only access
- Cannot modify data
- Cannot generate reports
- Read-only dashboard

## Troubleshooting

### Common Issues

#### Application Won't Start
- **Solution**: Reinstall .NET 9.0 Runtime
- Check system requirements
- Run as Administrator

#### Login Failed
- **Solution**: Verify credentials
- Check CAPS LOCK
- Contact administrator for password reset

#### Data Not Updating
- **Solution**: Click "Refresh Data" button
- Check internet connection
- Verify Arduino connection

#### Arduino Not Detected
- **Solution**: Check USB connection
- Verify COM port in Device Manager
- Install CH340 drivers if needed

#### Slow Performance
- **Solution**: Close other applications
- Check RAM usage
- Restart application

### Error Messages

| Error | Cause | Solution |
|-------|-------|----------|
| "Invalid credentials" | Wrong username/password | Re-enter credentials |
| "Connection failed" | Network issue | Check connection |
| "Arduino not found" | USB disconnected | Reconnect Arduino |
| "Sensor timeout" | Sensor malfunction | Check sensor wiring |

### Getting Help

**Technical Support:**
- Email: support@mukubaico.com
- Phone: +260-XXX-XXXX
- Hours: Monday-Friday, 8AM-5PM CAT

**Documentation:**
- User Guide: This document
- Arduino Integration: ARDUINO_INTEGRATION.md
- Quick Start: QUICKSTART.md
- Deployment: DEPLOYMENT.md

## Best Practices

### Daily Operations
1. Log in at start of shift
2. Review Main Dashboard
3. Check all compliance scores
4. Verify Arduino connection
5. Review any alerts or warnings
6. Generate daily reports
7. Log out at end of shift

### Weekly Tasks
1. Review all compliance pages
2. Check sensor calibration
3. Verify emergency systems
4. Update risk register
5. Generate weekly reports

### Monthly Tasks
1. Conduct emergency drills
2. Review training records
3. Update rehabilitation plans
4. Audit compliance scores
5. Generate monthly reports

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| Ctrl + R | Refresh Data |
| Ctrl + H | Go to Home |
| Ctrl + L | Logout |
| F5 | Refresh Page |
| Esc | Close Menu |

## Data Export

### Generating Reports
1. Navigate to desired compliance page
2. Click "📊 View Reports" button
3. Select report type
4. Choose date range
5. Click "Generate"
6. Save or print report

### Report Types
- Daily Compliance Report
- Weekly Summary Report
- Monthly Audit Report
- Annual Compliance Report
- Custom Date Range Report

## Security

### Password Policy
- Minimum 8 characters
- Change every 90 days
- No password reuse
- Lock after 3 failed attempts

### Session Management
- Auto-logout after 30 minutes of inactivity
- Manual logout recommended
- Single session per user

### Data Privacy
- All data encrypted
- Audit logs maintained
- Access controlled by role
- Compliance with GDPR

---
**Version**: 1.0.0  
**Last Updated**: December 2025  
**For**: Mukubaico Enviro-Safe TSF Dashboard
