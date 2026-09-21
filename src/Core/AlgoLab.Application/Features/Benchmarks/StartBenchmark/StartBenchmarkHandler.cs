using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.DTOs;
using AlgoLab.Domain.Entities;
using AlgoLab.Domain.Enums;

namespace AlgoLab.Application.Features.Benchmarks.StartBenchmark;

public class StartBenchmarkHandler
{
    private readonly IApplicationDbContext _db;
    private readonly IBenchmarkQueue _queue;

    public StartBenchmarkHandler(
        IApplicationDbContext db,
        IBenchmarkQueue queue)
    {
        _db = db;
        _queue = queue;
    }

    public async Task<Guid> HandleAsync(
        StartBenchmarkRequest request,
        CancellationToken cancellationToken)
    {
        // validation

        var session = new BenchmarkSession
        {
            AlgorithmId = request.AlgorithmId,
            StartN = request.StartN,
            EndN = request.EndN,
            Step = request.Step,
            ForceRecalculate = request.ForceRecalculate,
            Status = SessionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _db.BenchmarkSessions.Add(session);

        await _db.SaveChangesAsync(cancellationToken);

        await _queue.EnqueueAsync(
            session.Id,
            cancellationToken);

        return session.Id;
    }
}