using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Application.Features.Benchmarks.GetSessionDetails
{
    public class GetSessionDetailsHandler
    {
        private readonly IApplicationDbContext _db;

        public GetSessionDetailsHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<BenchmarkDetailsDto?> HandleAsync(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            return await _db.BenchmarkSessions
                .AsNoTracking()
                .Where(x => x.Id == sessionId)
                .Select(x => new BenchmarkDetailsDto(
                    x.Id,
                    x.Algorithm.Name,
                    x.Status.ToString(),
                    x.Runs
                        .OrderBy(r => r.N)
                        .Select(r => new BenchmarkPointDto(
                            r.N,
                            r.ExecutionTimeMs,
                            r.StepsCount,
                            r.FromCache
                        ))
                        .ToList()
                ))
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}