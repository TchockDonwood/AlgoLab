using AlgoLab.Application.Common.Models;

namespace AlgoLab.Application.Common.Interfaces
{
    public interface IBenchmarkRunner
    {
        Task<BenchmarkResult> MeasureAsync(
            IAlgorithm algorithm,
            GenerationRequest request,
            CancellationToken cancellationToken);
    }
}
