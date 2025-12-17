# Deployment Guide - TSF Safety & Compliance Dashboard

## Platform-Specific Deployment

### Windows Desktop Application

#### Build for Windows
```bash
dotnet publish -f net9.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64
```

#### Create Installer
1. Use Visual Studio Installer Projects extension
2. Or use WiX Toolset for MSI installer
3. Or use MSIX packaging for Microsoft Store

#### Distribution
- Direct download from company website
- Microsoft Store (requires MSIX package)
- Internal deployment via Group Policy

### Android Application

#### Build APK
```bash
dotnet publish -f net9.0-android -c Release
```

#### Create Signed APK
1. Generate keystore:
```bash
keytool -genkey -v -keystore mukubaico.keystore -alias mukubaico -keyalg RSA -keysize 2048 -validity 10000
```

2. Sign the APK in project properties or via command line

#### Distribution
- Google Play Store
- Internal company app store
- Direct APK distribution

### iOS Application

#### Build for iOS
```bash
dotnet publish -f net9.0-ios -c Release
```

#### Requirements
- macOS with Xcode
- Apple Developer Account ($99/year)
- Provisioning profiles and certificates

#### Distribution
- Apple App Store
- TestFlight for beta testing
- Enterprise distribution (requires Apple Enterprise account)

### macOS Application

#### Build for macOS
```bash
dotnet publish -f net9.0-maccatalyst -c Release
```

#### Distribution
- Mac App Store
- Direct download (.pkg or .dmg)
- Notarization required for distribution outside App Store

## Configuration for Production

### 1. Update App Settings

Create `appsettings.json`:
```json
{
  "IoTSettings": {
    "ApiEndpoint": "https://your-iot-platform.com/api",
    "ApiKey": "your-api-key",
    "RefreshInterval": 5000
  },
  "GISSettings": {
    "MapServiceUrl": "https://your-gis-service.com",
    "ApiKey": "your-gis-api-key"
  },
  "DatabaseSettings": {
    "ConnectionString": "your-connection-string"
  }
}
```

### 2. Environment Variables

Set these environment variables:
- `MUKUBAICO_API_KEY` - IoT platform API key
- `MUKUBAICO_GIS_KEY` - GIS service API key
- `MUKUBAICO_DB_CONNECTION` - Database connection string

### 3. Update Service URLs

Edit `Services/TSFDataService.cs`:
```csharp
private readonly string _apiEndpoint = Environment.GetEnvironmentVariable("MUKUBAICO_API_KEY") 
    ?? "https://default-api.com";
```

## Security Considerations

### 1. API Key Management
- Never hardcode API keys
- Use secure key storage (Azure Key Vault, AWS Secrets Manager)
- Rotate keys regularly

### 2. Data Encryption
- Enable HTTPS for all API calls
- Encrypt sensitive data at rest
- Use certificate pinning for mobile apps

### 3. Authentication
Implement user authentication:
```csharp
// Add to MauiProgram.cs
builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
```

### 4. Compliance
- GDPR compliance for EU users
- Data retention policies
- Audit logging

## Performance Optimization

### 1. Reduce App Size
```bash
# Enable trimming
dotnet publish -p:PublishTrimmed=true

# Enable AOT compilation (Android/iOS)
dotnet publish -p:UseInterpreter=false
```

### 2. Optimize Images
- Use compressed images
- Implement lazy loading
- Cache frequently used resources

### 3. Database Optimization
- Implement data caching
- Use pagination for large datasets
- Index frequently queried fields

## Monitoring & Analytics

### 1. Application Insights
```csharp
// Add to MauiProgram.cs
builder.Services.AddApplicationInsightsTelemetry();
```

### 2. Crash Reporting
- AppCenter for crash analytics
- Sentry for error tracking
- Custom logging service

### 3. Usage Analytics
- Track feature usage
- Monitor performance metrics
- User behavior analytics

## Continuous Integration/Deployment

### GitHub Actions Example
```yaml
name: Build and Deploy

on:
  push:
    branches: [ main ]

jobs:
  build:
    runs-on: windows-latest
    steps:
    - uses: actions/checkout@v2
    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: 9.0.x
    - name: Restore dependencies
      run: dotnet restore
    - name: Build
      run: dotnet build --configuration Release
    - name: Publish Windows
      run: dotnet publish -f net9.0-windows10.0.19041.0 -c Release
```

### Azure DevOps Pipeline
```yaml
trigger:
- main

pool:
  vmImage: 'windows-latest'

steps:
- task: UseDotNet@2
  inputs:
    version: '9.0.x'
- task: DotNetCoreCLI@2
  inputs:
    command: 'restore'
- task: DotNetCoreCLI@2
  inputs:
    command: 'build'
    configuration: 'Release'
```

## Testing Before Deployment

### 1. Unit Tests
```bash
dotnet test
```

### 2. UI Tests
- Use Appium for mobile testing
- WinAppDriver for Windows testing

### 3. Load Testing
- Test with simulated IoT data streams
- Verify performance under load
- Test auto-refresh with multiple users

## Rollback Strategy

### 1. Version Control
- Tag each release: `git tag v1.0.0`
- Maintain release branches
- Document changes in CHANGELOG.md

### 2. Backup
- Backup database before deployment
- Keep previous version available
- Test rollback procedure

### 3. Gradual Rollout
- Deploy to test environment first
- Beta testing with select users
- Phased rollout to production

## Post-Deployment Checklist

- [ ] Verify all API connections working
- [ ] Test real-time data refresh
- [ ] Confirm GIS map loading correctly
- [ ] Validate compliance calculations
- [ ] Test on all target platforms
- [ ] Verify alert notifications
- [ ] Check performance metrics
- [ ] Review security settings
- [ ] Update documentation
- [ ] Train end users
- [ ] Set up monitoring alerts
- [ ] Schedule regular backups

## Support & Maintenance

### Regular Updates
- Monthly security patches
- Quarterly feature updates
- Annual major version releases

### Monitoring Schedule
- Daily: Check error logs
- Weekly: Review performance metrics
- Monthly: Security audit
- Quarterly: Compliance review

### Contact Information
- Technical Support: support@mukubaico.com
- Emergency Hotline: +260-XXX-XXXX
- Documentation: https://docs.mukubaico.com

---
**Last Updated**: December 2025  
**Version**: 1.0.0  
**Deployment Status**: Ready for Production
