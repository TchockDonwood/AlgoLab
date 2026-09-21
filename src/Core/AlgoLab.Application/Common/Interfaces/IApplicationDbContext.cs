using AlgoLab.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Algorithm> Algorithms { get; }
    DbSet<BenchmarkSession> BenchmarkSessions { get; }
    DbSet<BenchmarkRun> BenchmarkRuns { get; }
    DbSet<SessionRun> SessionRuns { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}