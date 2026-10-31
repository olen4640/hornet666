using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Hornet666.App.Models;

namespace Hornet666.App;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<DensityLevel> _allLevels = new();

    public MainWindow()
    {
        InitializeComponent();
        LoadDemoData();
        ApplyFilters();
    }

    private void LoadDemoData()
    {
        // Synthetic examples only. Replace with a normalized live order-book feed.
        var samples = new[]
        {
            new DensityLevel { Symbol = "SBER", Side = "BID", Price = 287.50m, Volume = 18400, DistanceTicks = 2, PriceShiftTicks = 0, AgeSeconds = 184, Touches = 2, IsStable = true, IsRoundLevel = true, Behavior = "Объём держится" },
            new DensityLevel { Symbol = "SBER", Side = "ASK", Price = 288.10m, Volume = 9200, DistanceTicks = 8, PriceShiftTicks = 4, AgeSeconds = 38, Touches = 0, IsStable = false, IsRoundLevel = false, Behavior = "Перестановка" },
            new DensityLevel { Symbol = "GAZP", Side = "BID", Price = 164.00m, Volume = 25700, DistanceTicks = 3, PriceShiftTicks = 0, AgeSeconds = 426, Touches = 1, IsStable = true, IsRoundLevel = true, Behavior = "Пополнение после сделок" },
            new DensityLevel { Symbol = "LKOH", Side = "ASK", Price = 6840.00m, Volume = 1350, DistanceTicks = 5, PriceShiftTicks = 1, AgeSeconds = 91, Touches = 1, IsStable = true, IsRoundLevel = true, Behavior = "Стабильная заявка" },
            new DensityLevel { Symbol = "VTBR", Side = "BID", Price = 0.0254m, Volume = 8200000, DistanceTicks = 1, PriceShiftTicks = 7, AgeSeconds = 22, Touches = 0, IsStable = false, IsRoundLevel = false, Behavior = "Скользящий объём" },
            new DensityLevel { Symbol = "YDEX", Side = "ASK", Price = 4120.00m, Volume = 740, DistanceTicks = 11, PriceShiftTicks = 0, AgeSeconds = 302, Touches = 3, IsStable = true, IsRoundLevel = true, Behavior = "Частичное исполнение" },
            new DensityLevel { Symbol = "ROSN", Side = "BID", Price = 498.50m, Volume = 6100, DistanceTicks = 6, PriceShiftTicks = 0, AgeSeconds = 210, Touches = 1, IsStable = true, IsRoundLevel = false, Behavior = "Объём сохраняется" }
        };

        foreach (var sample in samples)
            _allLevels.Add(sample);
    }

    private void FilterChanged(object sender, RoutedEventArgs e) => ApplyFilters();

    private void ApplyFilters()
    {
        if (DensityGrid is null)
            return;

        decimal minVolume = 0;
        if (decimal.TryParse(MinVolumeBox.Text, out var parsed) && parsed > 0)
            minVolume = parsed;

        var stableOnly = StableOnlyCheck.IsChecked == true;
        var roundOnly = RoundOnlyCheck.IsChecked == true;

        var filtered = _allLevels.Where(level =>
            (!stableOnly || level.IsStable) &&
            (!roundOnly || level.IsRoundLevel) &&
            level.Volume >= minVolume)
            .OrderBy(level => level.DistanceTicks)
            .ThenByDescending(level => level.Volume)
            .ToList();

        DensityGrid.ItemsSource = filtered;
        CountText.Text = $"Показано: {filtered.Count} / {_allLevels.Count} уровней";
    }

    private void ResetFilters_Click(object sender, RoutedEventArgs e)
    {
        StableOnlyCheck.IsChecked = false;
        RoundOnlyCheck.IsChecked = false;
        MinVolumeBox.Text = "0";
        ApplyFilters();
    }
}
