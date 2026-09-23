namespace AlgoLab.Application.Common.Interfaces;

public interface IBenchmarkStatisticsService
{
    double GetMedian(List<double> values);

    (List<int> Ns, List<double> Times, List<int> Ms, List<int> OriginalIndices) FilterIqrOutliers(
        List<int> ns,
        List<double> times,
        List<int> ms,
        double factor = 1.5);

    (string ModelName, List<double> FitTimes) FindBestFitModel(List<int> ns, List<double> times);
}