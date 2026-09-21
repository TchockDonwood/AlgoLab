namespace AlgoLab.Application.Common.Models;

/// <summary>Результат одного замера. Ровно одно из полей заполнено.</summary>
public sealed record BenchmarkResult(double? TimeMs, long? Steps)
{
    public static BenchmarkResult FromTime(TimeSpan elapsed)
        => new(elapsed.TotalMilliseconds, null);

    public static BenchmarkResult FromSteps(long steps)
        => new(null, steps);
}