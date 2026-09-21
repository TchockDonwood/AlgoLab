using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Application.Features.Benchmarks.GetHistory
{
    public class GetHistoryHandler
    {
        private readonly IApplicationDbContext _db;

        public GetHistoryHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<BenchmarkSessionDto>>
            HandleAsync(CancellationToken cancellationToken)
        {
            return await _db.BenchmarkSessions
                .AsNoTracking()
                .Include(x => x.Algorithm)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new BenchmarkSessionDto(
                    x.Id,
                    x.AlgorithmId,
                    x.Algorithm.Name,
                    x.StartN,
                    x.EndN,
                    x.Step,
                    x.Status.ToString(),
                    x.CreatedAt,
                    x.StartedAt,
                    x.FinishedAt
                ))
                .ToListAsync(cancellationToken);
        }
    }
}
