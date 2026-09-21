using AlgoLab.Domain.Entities;

namespace AlgoLab.Application.Common.Interfaces
{
    public interface IBenchmarkResultStore
    {
        Task<BenchmarkRun?> GetAsync(
            Guid algorithmId,
            int n,
            CancellationToken cancellationToken);

        Task SaveAsync(
            BenchmarkRun result,
            CancellationToken cancellationToken);
    }
}