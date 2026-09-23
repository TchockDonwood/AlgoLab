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
        private readonly IBenchmarkCancellationManager _cancellationManager;
        private readonly IBenchmarkStatisticsService _statistics;

        public BenchmarkExecutor(
            AppDbContext db,
            IBenchmarkResultStore resultStore,
            IBenchmarkRunner runner,
            IAlgorithmRegistry algorithms,
            IBenchmarkCancellationManager cancellationManager,
            IBenchmarkStatisticsService statistics)
        {
            _db = db;
            _resultStore = resultStore;
            _runner = runner;
            _algorithms = algorithms;
            _cancellationManager = cancellationManager;
            _statistics = statistics;
        }

        public async Task ExecuteAsync(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            var sessionToken = _cancellationManager.GetToken(sessionId);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, sessionToken);
            var ct = linkedCts.Token;

            var session = await _db.BenchmarkSessions
                .Include(s => s.Algorithm)
                .FirstOrDefaultAsync(s => s.Id == sessionId, ct);

            if (session is null)
                return;

            if (session.Status == SessionStatus.Cancelled)
                return;

            session.Status = SessionStatus.Running;
            session.StartedAt = DateTime.UtcNow;
            session.FinishedAt = null;
            session.ErrorMessage = null;
            await _db.SaveChangesAsync(ct);

            try
            {
                var algorithmEntity = session.Algorithm
                    ?? await _db.Algorithms.FirstOrDefaultAsync(
                        a => a.Id == session.AlgorithmId, ct);

                if (algorithmEntity is null)
                {
                    session.Status = SessionStatus.Failed;
                    session.ErrorMessage = $"Algorithm with id {session.AlgorithmId} not found.";
                    session.FinishedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync(CancellationToken.None);
                    return;
                }

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
                    await _db.SaveChangesAsync(CancellationToken.None);
                    return;
                }

                var ns = new List<int>();
                var ms = new List<int>();
                var times = new List<double>();
                var sessionRuns = new List<SessionRun>();

                bool is2D = algorithmEntity.InputArity == 2;
                int startM = session.StartM ?? 1;
                int endM = session.EndM ?? session.EndN;

                for (var n = session.StartN; n <= session.EndN; n += session.Step)
                {
                    ct.ThrowIfCancellationRequested();

                    if (is2D)
                    {
                        for (var m = startM; m <= endM; m += session.Step)
                        {
                            ct.ThrowIfCancellationRequested();
                            await ProcessPoint(session, algorithmImpl, algorithmEntity.Id, n, m, ct, ns, ms, times, sessionRuns);
                        }
                    }
                    else
                    {
                        await ProcessPoint(session, algorithmImpl, algorithmEntity.Id, n, null, ct, ns, ms, times, sessionRuns);
                    }
                }

                // Фильтрация выбросов и аппроксимация (только для 1D)
                if (!is2D && times.Count > 0)
                {
                    var (filteredNs, filteredTimes, _, originalIndices) =
                        _statistics.FilterIqrOutliers(ns, times, ms);

                    // Помечаем выбросы
                    for (int i = 0; i < sessionRuns.Count; i++)
                    {
                        sessionRuns[i].IsOutlier = !originalIndices.Contains(i);
                    }

                    // Аппроксимация только по отфильтрованным данным
                    if (filteredTimes.Count >= 2)
                    {
                        var (modelName, fitTimes) = _statistics.FindBestFitModel(filteredNs, filteredTimes);
                        session.ApproximationModel = modelName;

                        // Сохраняем точки аппроксимации (можно добавить в БД при необходимости)
                    }
                }

                await _db.SaveChangesAsync(CancellationToken.None);

                session.Status = SessionStatus.Completed;
                session.FinishedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(CancellationToken.None);
            }
            catch (OperationCanceledException)
            {
                session.Status = SessionStatus.Cancelled;
                session.FinishedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                session.Status = SessionStatus.Failed;
                session.ErrorMessage = ex.Message;
                session.FinishedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync(CancellationToken.None);
                throw;
            }
            finally
            {
                _cancellationManager.Complete(sessionId);
            }
        }

        private async Task ProcessPoint(
            BenchmarkSession session,
            IAlgorithm algorithmImpl,
            Guid algorithmId,
            int n,
            int? m,
            CancellationToken ct,
            List<int> ns,
            List<int> ms,
            List<double> times,
            List<SessionRun> sessionRuns)
        {
            var cachedRun = await _resultStore.GetAsync(algorithmId, n, m, ct);
            BenchmarkRun benchmarkRun;
            bool fromCache;

            if (cachedRun is not null && !session.ForceRecalculate)
            {
                benchmarkRun = cachedRun;
                fromCache = true;
            }
            else
            {
                var seed = Random.Shared.Next();
                var request = m.HasValue
                    ? GenerationRequest.Pair(n, m.Value, seed)
                    : GenerationRequest.Single(n, seed);

                var result = await _runner.MeasureAsync(algorithmImpl, request, ct);

                if (cachedRun is not null)
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
                        AlgorithmId = algorithmId,
                        N = n,
                        M = m,
                        ExecutionTimeMs = result.TimeMs,
                        StepsCount = result.Steps,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _db.BenchmarkRuns.Add(benchmarkRun);
                }
                await _db.SaveChangesAsync(ct);
                fromCache = false;
            }

            var sessionRun = new SessionRun
            {
                Id = Guid.NewGuid(),
                SessionId = session.Id,
                BenchmarkRunId = benchmarkRun.Id,
                N = n,
                M = m,
                ExecutionTimeMs = benchmarkRun.ExecutionTimeMs,
                StepsCount = benchmarkRun.StepsCount,
                FromCache = fromCache,
                IsOutlier = false
            };

            _db.SessionRuns.Add(sessionRun);
            await _db.SaveChangesAsync(ct);

            sessionRuns.Add(sessionRun);
            ns.Add(n);
            ms.Add(m ?? 0);
            times.Add(benchmarkRun.ExecutionTimeMs ?? 0);
        }
    }
}