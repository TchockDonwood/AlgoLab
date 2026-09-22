using AlgoLab.Application.Common.Interfaces;
using AlgoLab.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AlgoLab.Application.Features.Benchmarks.CancelBenchmark
{
    public class CancelBenchmarkHandler
    {
        private readonly IApplicationDbContext _db;

        public CancelBenchmarkHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task HandleAsync(
            Guid sessionId,
            CancellationToken cancellationToken)
        {
            // 1. Загрузить сессию
            var session = await _db.BenchmarkSessions
                .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);

            if (session is null)
                throw new KeyNotFoundException(
                    $"Benchmark session {sessionId} not found.");

            // 2. Если уже завершена — отменять нечего
            if (session.Status is SessionStatus.Completed
                or SessionStatus.Cancelled
                or SessionStatus.Failed)
            {
                return; // идемпотентно
            }

            // 3. Пометить как отменённую
            session.Status = SessionStatus.Cancelled;
            session.FinishedAt = DateTime.UtcNow;

            // 4. Сохранить
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}