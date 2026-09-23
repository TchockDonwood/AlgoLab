using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Infrastructure.Benchmarking;

public class BenchmarkStatistics : IBenchmarkStatisticsService
{
    public double GetMedian(List<double> values)
    {
        if (values == null || values.Count == 0) return 0;
        var sorted = values.OrderBy(x => x).ToList();
        int count = sorted.Count;
        if (count % 2 == 0)
            return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
        return sorted[count / 2];
    }

    public (List<int> Ns, List<double> Times, List<int> Ms, List<int> OriginalIndices) FilterIqrOutliers(
        List<int> ns,
        List<double> times,
        List<int> ms,
        double factor = 1.5)
    {
        if (times.Count == 0)
            return (ns, times, ms, Enumerable.Range(0, ns.Count).ToList());

        var sorted = times.OrderBy(x => x).ToList();
        double q1 = GetPercentile(sorted, 25);
        double q3 = GetPercentile(sorted, 75);
        double iqr = q3 - q1;
        double lowerBound = q1 - factor * iqr;
        double upperBound = q3 + factor * iqr;

        var fNs = new List<int>();
        var fTimes = new List<double>();
        var fMs = new List<int>();
        var originalIndices = new List<int>();

        for (int i = 0; i < times.Count; i++)
        {
            if (times[i] >= lowerBound && times[i] <= upperBound)
            {
                fNs.Add(ns[i]);
                fTimes.Add(times[i]);
                fMs.Add(ms[i]);
                originalIndices.Add(i);
            }
        }
        return (fNs, fTimes, fMs, originalIndices);
    }

    private double GetPercentile(List<double> sorted, double percentile)
    {
        if (sorted.Count == 0) return 0;
        if (sorted.Count == 1) return sorted[0];

        double index = (percentile / 100.0) * (sorted.Count - 1);
        int lower = (int)Math.Floor(index);
        int upper = (int)Math.Ceiling(index);

        if (lower == upper) return sorted[lower];
        return sorted[lower] + (sorted[upper] - sorted[lower]) * (index - lower);
    }

    public (string ModelName, List<double> FitTimes) FindBestFitModel(List<int> ns, List<double> times)
    {
        var models = new Dictionary<string, Func<double, double>>
        {
            { "O(1)", n => 1 },
            { "O(log n)", n => Math.Log(n) },
            { "O(n)", n => n },
            { "O(n log n)", n => n * Math.Log(n) },
            { "O(n^2)", n => n * n },
            { "O(n^3)", n => n * n * n }
        };

        string bestModel = "O(1)";
        double bestError = double.MaxValue;
        List<double> bestFit = new();

        double[] xData = ns.Select(n => (double)n).ToArray();
        double[] yData = times.ToArray();

        foreach (var kv in models)
        {
            double[] transformedX = xData.Select(kv.Value).ToArray();

            // Метод наименьших квадратов: y = c * f(x)
            double sumXY = 0, sumX2 = 0;
            for (int i = 0; i < transformedX.Length; i++)
            {
                sumXY += transformedX[i] * yData[i];
                sumX2 += transformedX[i] * transformedX[i];
            }
            double constantFactor = sumX2 == 0 ? 0 : sumXY / sumX2;

            double mse = 0;
            var yFit = new List<double>();
            for (int i = 0; i < xData.Length; i++)
            {
                double fit = constantFactor * kv.Value(xData[i]);
                yFit.Add(fit);
                mse += Math.Pow(yData[i] - fit, 2);
            }
            mse /= yData.Length;

            if (mse < bestError)
            {
                bestError = mse;
                bestModel = kv.Key;
                bestFit = yFit;
            }
        }
        return (bestModel, bestFit);
    }
}