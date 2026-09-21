using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Entities;
using AlgoLab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Infrastructure.Benchmarking
{
    public class BenchmarkResultStore : IBenchmarkResultStore
    {
        private readonly AppDbContext _db;

        public BenchmarkResultStore(AppDbContext db)
        {
            _db = db;
        }

        public Task<BenchmarkRun?> GetAsync(
            Guid algorithmId,
            int n,
            CancellationToken cancellationToken)
        {
            return _db.BenchmarkRuns
                .FirstOrDefaultAsync(
                    x => x.AlgorithmId == algorithmId &&
                         x.N == n,
                    cancellationToken);
        }

        public async Task SaveAsync(
            BenchmarkRun result,
            CancellationToken cancellationToken)
        {
            _db.BenchmarkRuns.Add(result);

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
