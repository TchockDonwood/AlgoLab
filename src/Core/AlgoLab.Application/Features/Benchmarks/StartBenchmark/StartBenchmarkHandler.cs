using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.DTOs;
using AlgoLab.Domain.Entities;
using AlgoLab.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
        await ValidateAsync(request, cancellationToken);

        var session = new BenchmarkSession
        {
            AlgorithmId = request.AlgorithmId,
            StartN = request.StartN,
            EndN = request.EndN,
            StartM = request.StartM,
            EndM = request.EndM,
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

    private async Task ValidateAsync(
    StartBenchmarkRequest request,
    CancellationToken cancellationToken)
    {
        // 1. AlgorithmId
        if (request.AlgorithmId == Guid.Empty)
            throw new ValidationException("AlgorithmId is required.");

        // 2. N-диапазон
        if (request.StartN < 0)
            throw new ValidationException("StartN must be non-negative.");

        if (request.EndN < request.StartN)
            throw new ValidationException("EndN must be greater than or equal to StartN.");

        if (request.Step <= 0)
            throw new ValidationException("Step must be positive.");

        // 3. M-диапазон (если алгоритм двумерный)
        // Предполагаем, что StartM и EndM — int (не nullable).
        // Если для одномерных алгоритмов они не нужны, эту проверку можно
        // делать только после определения типа алгоритма.
        if (request.StartM < 0)
            throw new ValidationException("StartM must be non-negative.");

        if (request.EndM < request.StartM)
            throw new ValidationException("EndM must be greater than or equal to StartM.");

        // 5. Проверка существования алгоритма
        var algorithmExists = await _db.Algorithms
            .AnyAsync(a => a.Id == request.AlgorithmId, cancellationToken);

        if (!algorithmExists)
            throw new ValidationException($"Algorithm with id {request.AlgorithmId} not found.");
    }
}