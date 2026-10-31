namespace Hornet666.App.Models;

public sealed class DensityLevel
{
    public string Symbol { get; init; } = "";
    public string Side { get; init; } = "";
    public decimal Price { get; init; }
    public decimal Volume { get; init; }
    public decimal DistanceTicks { get; init; }
    public decimal PriceShiftTicks { get; init; }
    public int AgeSeconds { get; init; }
    public int Touches { get; init; }
    public bool IsStable { get; init; }
    public bool IsRoundLevel { get; init; }
    public string Behavior { get; init; } = "";

    public string StabilityLabel => IsStable ? "СТОИТ" : "ДВИГАЕТСЯ";
    public string RoundLabel => IsRoundLevel ? "ДА" : "—";
}
