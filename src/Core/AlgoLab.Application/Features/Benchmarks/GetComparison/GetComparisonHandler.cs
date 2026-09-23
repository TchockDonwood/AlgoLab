using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Application.Features.Benchmarks.GetComparison
{
    public class GetComparisonHandler
    {
        private readonly IApplicationDbContext _db;

        public GetComparisonHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IReadOnlyList<BenchmarkSeriesDto>> HandleAsync(
            IReadOnlyCollection<Guid> sessionIds,
            CancellationToken cancellationToken)
        {
            if (sessionIds.Count == 0)
                return Array.Empty<BenchmarkSeriesDto>();

            var orderedIds = sessionIds.Distinct().ToList();

            var sessions = await _db.BenchmarkSessions
                .AsNoTracking()
                .Where(s => orderedIds.Contains(s.Id))
                .Select(s => new
                {
                    s.Id,
                    s.AlgorithmId,
                    AlgorithmName = s.Algorithm.Name,
                    Points = s.Runs
                        .OrderBy(r => r.N)
                        .ThenBy(r => r.M)
                        .Select(r => new BenchmarkPointDto(
                            r.N,
                            r.M,
                            r.ExecutionTimeMs,
                            r.StepsCount,
                            r.FromCache,
                            r.IsOutlier
                        ))
                        .ToList()
                })
                .ToListAsync(cancellationToken);

            var byId = sessions.ToDictionary(s => s.Id);

            var result = new List<BenchmarkSeriesDto>(orderedIds.Count);
            foreach (var id in orderedIds)
            {
                if (byId.TryGetValue(id, out var s))
                {
                    result.Add(new BenchmarkSeriesDto(
                        s.Id,
                        s.AlgorithmId,
                        s.AlgorithmName,
                        s.Points
                    ));
                }
                else
                {
                    result.Add(new BenchmarkSeriesDto(
                        id,
                        Guid.Empty,
                        "Unknown",
                        Array.Empty<BenchmarkPointDto>()
                    ));
                }
            }
            return result;
        }
    }
}