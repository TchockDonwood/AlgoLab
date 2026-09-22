using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.Common.Models;
using AlgoLab.Domain.Entities;
using AlgoLab.Domain.Enums;
using AlgoLab.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Infrastructure.Benchmarking
{
    public class BenchmarkExecutor : IBenchmarkExecutor
    {
        private readonly AppDbContext _db;
        private readonly IBenchmarkResultStore _resultStore;
        private readonly IBenchmarkRunner _runner;
        private readonly IAlgorithmRegistry _algorithms;

        public BenchmarkExecutor(
            AppDbContext db,
            IBenchmarkResultStore resultStore,
            IBenchmarkRunner runner,
            IAlgorithmRegistry algorithms)
        {
            _db = db;
            _resultStore = resultStore;
            _runner = runner;
            _algorithms = algorithms;
        }

        public async Task ExecuteAsync(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            // 1. Получить сессию

            // 2. Проверить, не отменена ли она

            // 3. Поставить RUNNING

            // 4. Получить algorithm

            // 5. Найти реализацию в registry

            // 6. Цикл N

            // 7. Проверить cache

            // 8. Если cache нет → запустить algorithm

            // 9. Создать SessionRun

            // 10. COMPLETED


            // 1. Получить сессию
            var session = await _db.BenchmarkSessions
                .Include(s => s.Algorithm)
                .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

            if (session is null)
                return;

            // 2. Проверить, не отменена ли она
            if (session.Status == SessionStatus.Cancelled)
                return;

            // 3. Поставить RUNNING
            session.Status = SessionStatus.Running;
            session.StartedAt = DateTime.UtcNow;
            session.FinishedAt = null;
            session.ErrorMessage = null;
            await _db.SaveChangesAsync(cancellationToken);

            try
            {
                // 4. Получить algorithm
                var algorithmEntity = session.Algorithm
                    ?? await _db.Algorithms.FirstOrDefaultAsync(
                        a => a.Id == session.AlgorithmId, cancellationToken);

                if (algorithmEntity is null)
                {
                    session.Status = SessionStatus.Failed;
                    session.ErrorMessage = $"Algorithm with id {session.AlgorithmId} not found.";
                    session.FinishedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync(cancellationToken);
                    return;
                }

                // 5. Найти реализацию в registry
                IAlgorithm algorithmImpl;
                try
                {
                    algorithmImpl = _algorithms.Get(algorithmEntity.Code);
                }
                catch (Exception ex)
                {
                    session.Status = SessionStatus.Failed;
                    session.ErrorMessage = $"Algorithm implementation '{algorithmEntity.Code}' not found: {ex.Message}";
                    session.FinishedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync(cancellationToken);
                    return;
                }

                // 6. Цикл N
                for (var n = session.StartN; n <= session.EndN; n += session.Step)
                {
                    // 7. Проверить cache
                    var cachedRun = await _resultStore.GetAsync(
                        session.AlgorithmId, n, cancellationToken);

                    BenchmarkRun benchmarkRun;
                    bool fromCache;

                    if (cachedRun is not null && !session.ForceRecalculate)
                    {
                        // Используем кэш
                        benchmarkRun = cachedRun;
                        fromCache = true;
                    }
                    else
                    {
                        // 8. Если cache нет → запустить algorithm
                        var m = n; // предполагаем M = N для двумерных
                        var seed = Random.Shared.Next();
                        var request = GenerationRequest.Pair(n, m, seed);

                        var result = await _runner.MeasureAsync(
                            algorithmImpl, request, cancellationToken);

                        if (cachedRun is not null) // ForceRecalculate = true
                        {
                            cachedRun.ExecutionTimeMs = result.TimeMs;
                            cachedRun.StepsCount = result.Steps;
                            cachedRun.UpdatedAt = DateTime.UtcNow;
                            benchmarkRun = cachedRun;
                        }
                        else
                        {
                            benchmarkRun = new BenchmarkRun
                            {
                                Id = Guid.NewGuid(),
                                AlgorithmId = session.AlgorithmId,
                                N = n,
                                ExecutionTimeMs = result.TimeMs,
                                StepsCount = result.Steps,
                                CreatedAt = DateTime.UtcNow,
                                UpdatedAt = DateTime.UtcNow
                            };
                            _db.BenchmarkRuns.Add(benchmarkRun);
                        }

                        await _db.SaveChangesAsync(cancellationToken);
                        fromCache = false;
                    }

                    // 9. Создать SessionRun
                    var sessionRun = new SessionRun
                    {
                        Id = Guid.NewGuid(),
                        SessionId = session.Id,
                        BenchmarkRunId = benchmarkRun.Id,
                        N = n,
                        ExecutionTimeMs = benchmarkRun.ExecutionTimeMs,
                        StepsCount = benchmarkRun.StepsCount,
                        FromCache = fromCache
                    };

                    _db.SessionRuns.Add(sessionRun);
                    await _db.SaveChangesAsync(cancellationToken);
                }

                // 10. COMPLETED
                session.Status = SessionStatus.Completed;
                session.FinishedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                session.Status = SessionStatus.Failed;
                session.ErrorMessage = ex.Message;
                session.FinishedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(CancellationToken.None);
                throw;
            }
        }
    }
}