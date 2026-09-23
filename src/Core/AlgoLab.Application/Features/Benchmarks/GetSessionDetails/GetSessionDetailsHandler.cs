using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Application.Features.Benchmarks.GetSessionDetails
{
    public class GetSessionDetailsHandler
    {
        private readonly IApplicationDbContext _db;

        private readonly IBenchmarkStatisticsService _statistics;

        public GetSessionDetailsHandler(
            IApplicationDbContext db,
            IBenchmarkStatisticsService statistics)
        {
            _db = db;
            _statistics = statistics;
        }

        public async Task<BenchmarkDetailsDto?> HandleAsync(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            var session = await _db.BenchmarkSessions
                .AsNoTracking()
                .Include(x => x.Algorithm)
                .Where(x => x.Id == sessionId)
                .Select(x => new
                {
                    x.Id,
                    AlgorithmName = x.Algorithm.Name,
                    InputArity = x.Algorithm.InputArity,
                    x.Status,
                    x.ApproximationModel,
                    Runs = x.Runs.OrderBy(r => r.N).ThenBy(r => r.M).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (session == null)
                return null;

            var points = session.Runs.Select(r => new BenchmarkPointDto(
                r.N,
                r.M,
                r.ExecutionTimeMs,
                r.StepsCount,
                r.FromCache,
                r.IsOutlier
            )).ToList();

            // Для 1D алгоритмов создаем точки аппроксимации
            List<double>? approximationPoints = null;
            if (session.InputArity == 1 && !string.IsNullOrEmpty(session.ApproximationModel))
            {
                var validPoints = points.Where(p => !p.IsOutlier).ToList();
                if (validPoints.Count >= 2)
                {
                    var ns = validPoints.Select(p => p.N).ToList();
                    var times = validPoints.Select(p => p.ExecutionTimeMs ?? 0).ToList();
                    var (_, fitTimes) = _statistics.FindBestFitModel(ns, times);
                    approximationPoints = fitTimes;
                }
            }

            return new BenchmarkDetailsDto(
                session.Id,
                session.AlgorithmName,
                session.InputArity,
                session.Status.ToString(),
                session.ApproximationModel,
                approximationPoints,
                points
            );
        }
    }
}