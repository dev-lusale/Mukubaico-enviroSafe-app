# Build Windows Only - Skip Android Errors

## Quick Fix for Android Build Errors

If you're getting Android build errors and only need Windows:

### Option 1: Build Windows Only (Recommended)

Always specify the Windows target framework:

```bash
# Build Windows only
dotnet build -f net9.0-windows10.0.19041.0

# Run Windows only
dotnet run -f net9.0-windows10.0.19041.0

# Publish Windows only
dotnet publish -f net9.0-windows10.0.19041.0 -c Release
```

**This completely avoids Android build issues!**

### Option 2: Temporarily Disable Android

Create a backup and modify `.csproj`:

#### Backup Current File
```bash
copy MukubaicoTSFDashboard.csproj MukubaicoTSFDashboard.csproj.backup
```

#### Edit MukubaicoTSFDashboard.csproj

Change this:
```xml
<TargetFrameworks>net9.0-android;net9.0-ios;net9.0-maccatalyst</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net9.0-windows10.0.19041.0</TargetFrameworks>
```

To this:
```xml
<!-- Temporarily build Windows only -->
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">net9.0-windows10.0.19041.0</TargetFrameworks>
<!-- Original: net9.0-android;net9.0-ios;net9.0-maccatalyst -->
```

Then build normally:
```bash
dotnet build
```

### Option 3: Use Visual Studio

1. Open `MukubaicoTSFDashboard.csproj` in Visual Studio 2022
2. Select "Windows Machine" as target
3. Press F5

Visual Studio will only build Windows target.

---

## Why This Works

### The Problem
- Android build requires Java/Android SDK
- Java configuration issues cause build failures
- File locking during Android build

### The Solution
- Build only Windows target
- Skip Android/iOS/macOS entirely
- No Java/Android SDK needed

### The Result
- ✅ Fast builds (7-8 seconds)
- ✅ No Android errors
- ✅ Fully functional Windows app
- ✅ Ready for deployment

---

## Restore Multi-Platform Later

When you want to build Android again:

### Restore from Backup
```bash
copy MukubaicoTSFDashboard.csproj.backup MukubaicoTSFDashboard.csproj
```

### Or Re-enable Manually
Change back to:
```xml
<TargetFrameworks>net9.0-android;net9.0-ios;net9.0-maccatalyst</TargetFrameworks>
<TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net9.0-windows10.0.19041.0</TargetFrameworks>
```

---

## Recommended Approach

### For Development & Testing
```bash
# Always specify Windows target
dotnet run -f net9.0-windows10.0.19041.0
```

### For Production Deployment
```bash
# Publish Windows version
dotnet publish -f net9.0-windows10.0.19041.0 -c Release -o ./publish
```

### For Android (When Ready)
1. Fix Java/Android SDK issues
2. Or use Visual Studio 2022
3. Or build on macOS
4. Then build Android target

---

## Summary

**Problem**: Android build errors  
**Solution**: Build Windows only  
**Command**: `dotnet build -f net9.0-windows10.0.19041.0`  
**Result**: ✅ Works perfectly!

**No need to fix Android errors if you only need Windows!**

