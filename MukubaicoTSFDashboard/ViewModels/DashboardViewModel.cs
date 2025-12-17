using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MukubaicoTSFDashboard.Models;
using MukubaicoTSFDashboard.Services;

namespace MukubaicoTSFDashboard.ViewModels;

public class DashboardViewModel : INotifyPropertyChanged
{
    private readonly TSFDataService _dataService;
    private TSFData _currentData;
    private bool _isRefreshing;

    public DashboardViewModel()
    {
        _dataService = new TSFDataService();
        _currentData = _dataService.GetLiveData();
        RefreshCommand = new Command(async () => await RefreshDataAsync());
        
        // Auto-refresh every 5 seconds
        StartAutoRefresh();
    }

    public TSFData CurrentData
    {
        get => _currentData;
        set { _currentData = value; OnPropertyChanged(); }
    }

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set { _isRefreshing = value; OnPropertyChanged(); }
    }

    public Command RefreshCommand { get; }

    public string RiskScoreColor => CurrentData.AlertLevel switch
    {
        "Green" => "#4CAF50",
        "Amber" => "#FF9800",
        "Red" => "#F44336",
        _ => "#4CAF50"
    };

    private async Task RefreshDataAsync()
    {
        IsRefreshing = true;
        await Task.Delay(500);
        CurrentData = _dataService.GetLiveData();
        OnPropertyChanged(nameof(RiskScoreColor));
        IsRefreshing = false;
    }

    private async void StartAutoRefresh()
    {
        while (true)
        {
            await Task.Delay(5000);
            if (!IsRefreshing)
            {
                await RefreshDataAsync();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
